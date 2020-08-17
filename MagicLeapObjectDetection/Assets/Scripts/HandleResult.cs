using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandleResult : MonoBehaviour
{
    public static HandleResult instance;
    public Renderer converter = null;
    void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class Rectangle
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }

    [System.Serializable]
    public class DetectedObject
    {
        public Rectangle rectangle;
        public string objectName;
        public double confidence;
    }

    [System.Serializable]
    public class DetectionResponse
    {
        public DetectedObject[] objectNames;
        public string requestId;
        public object metadata;

    }

    private List<string> labelsTexts = new List<string>();

    public void HandleJson(byte[] imageBytes, System.String jsonResponse, Matrix4x4 cameraToWorldMatrix, MLRaycast.QueryParams raycastParams)
    { 
        jsonResponse = jsonResponse.Replace("object", "objectName");
        DetectionResponse det = new DetectionResponse();
        det = JsonUtility.FromJson<DetectionResponse>(jsonResponse);
        Debug.Log("process Response");
        ResultAsText.instance.Show(" Handle Json");
        foreach (DetectedObject obj in det.objectNames)
        {
            Debug.Log(obj.objectName);
            int x = obj.rectangle.x;
            int y = obj.rectangle.y;
            ResultAsText.instance.Add(obj.objectName);
            labelsTexts.Add(obj.objectName);
            cast(x, y, cameraToWorldMatrix, raycastParams,spherePrefab);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(p, 0.2F);
        }
        markEdges(cameraToWorldMatrix, raycastParams);
        ShowImage(imageBytes);
    }

    public void ShowImage(byte[] imageData)
    {
        Texture2D texture = new Texture2D(8, 8);
        bool status = texture.LoadImage(imageData);

        if (status && (texture.width != 8 && texture.height != 8))
        {
            Camera cam = Camera.main;
            float cheight = 2f * cam.orthographicSize;
            float cwidth = cheight * cam.aspect;
            Debug.Log(cheight + " " + cwidth);
            Renderer renderer = converter;
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
            }
            ResultAsText.instance.Add("camera h, w: ("+cheight + ", " + cwidth + ") texture h, w: (" +texture.height+", "+ texture.width+")");
        }
    }
    public GameObject spherePrefab;
    public GameObject redsphere;
    public GameObject bluespherePrefab;
    private float xmin = 0;
    private float xmax = 1920;
    private float ymin = 0;
    private float ymax = 1080;

    private float umin = -0.155F;//0.29F;
    private float umax = 0.155F;
    private float vmin = 0.1F;//-0.216F;
    private float vmax = -0.1F;
    public void markEdges(Matrix4x4 m)
    {
        markEdges(m, Raycast.instance.CreateRaycastParams());
    }
    public void markEdges(Matrix4x4 m, MLRaycast.QueryParams raycastParams)
    {
        cast(xmin, ymin, m, raycastParams,bluespherePrefab);
        cast(xmin, ymax, m, raycastParams, bluespherePrefab);
        cast(xmax, ymin, m, raycastParams, bluespherePrefab);
        cast(xmax, ymax, m, raycastParams, bluespherePrefab);
        //Instantiate(bluespherePrefab, m.MultiplyPoint(new Vector3(-0.15F, -0.1F, -0.4F)), Quaternion.identity);
    }
    public void cast(float x, float y, Matrix4x4 m, MLRaycast.QueryParams raycastParams ,GameObject sphere)
    {
        //Debug.Log("ymin "+ymin);
        //move pointer basen on photo pixel position (1080/1920)
        if (x > xmax || x < xmin || y > ymax || y < ymin)
        {
            ResultAsText.instance.Add(x + " " + y + " is out of view, skipped");
            Debug.Log(x + " " + y + " is out of view, skipped");
            return;
        }
        Vector3 p = m.MultiplyPoint(new Vector3(u(x), v(y), -0.4F));

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(p); //care the camera might not look in the right spot anymore! todo
        if (Physics.Raycast(ray, out hit)){
            Transform objectHit = hit.transform;
            Instantiate(redsphere, objectHit.position, Quaternion.identity);
            Debug.Log("raycast hit with world. added red sphere " + objectHit.position);
            ResultAsText.instance.Add("raycast hit with world. added red sphere "+objectHit.position);
        }
        GameObject sphere2 = Instantiate(sphere, p, Quaternion.identity);
        ResultAsText.instance.Add(u(x) + " " + u(y) + " object marked");
        //Debug.Log(x+" "+y+" "+u(x) + " " + u(y) + " object marked");
        //sphere.transform.position = p;
    }
    private float u(float x)
    {
        float slope = ((umax - umin) / (xmax - xmin));
        float b = umin - slope * xmin;
        return slope * x + b;
    }
    private float v(float y)
    {
        float slope = ((vmax - vmin) / (ymax - ymin));
        float b = vmin - slope * ymin;
        return slope * y + b;
    }
}
