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



    public void HandleJson(byte[] imageBytes, System.String jsonResponse, CameraPosition cpos)
    { 
        jsonResponse = jsonResponse.Replace("object", "objectName");
        DetectionResponse det = new DetectionResponse();
        det = JsonUtility.FromJson<DetectionResponse>(jsonResponse);
        ResultAsText.instance.Show(" Handle Json");
        foreach (DetectedObject obj in det.objectNames)
        {
            Debug.Log(obj.objectName);
            int x = obj.rectangle.x;
            int y = obj.rectangle.y;
            ResultAsText.instance.Add(obj.objectName);
            Cast(x, y, cpos,spherePrefab, obj.objectName);
        }
        //markEdges(cpos);
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
        markEdges(new CameraPosition(Camera.main));
    }
    public void markEdges(CameraPosition cpos)
    {
        Cast(xmin, ymin, cpos,bluespherePrefab,"edge up left");
        Cast(xmin, ymax, cpos, bluespherePrefab,"edge down left");
        Cast(xmax, ymin, cpos, bluespherePrefab,"edge up right");
        Cast(xmax, ymax, cpos, bluespherePrefab,"edge down right");
        //Instantiate(bluespherePrefab, m.MultiplyPoint(new Vector3(-0.15F, -0.1F, -0.4F)), Quaternion.identity);
    }
    public void Cast(float x, float y, CameraPosition cpos,GameObject sphere, string name)
    {
        LabelCreater.instance.AddLabel(name);
        //Debug.Log("ymin "+ymin);
        //move pointer basen on photo pixel position (1080/1920)
        if (x > xmax || x < xmin || y > ymax || y < ymin)
        {
            ResultAsText.instance.Add(x + " " + y + " is out of view, skipped");
            Debug.Log(x + " " + y + " is out of view, skipped");
            return;
        }
        Vector3 p = cpos.cameratoWorldMatrix.MultiplyPoint(new Vector3(U(x), V(y), -0.4F));

        Raycast.instance.StartCast(Raycast.instance.CreateRaycastParams(cpos.ctransform, p));

        //GameObject sphere2 = Instantiate(sphere, p, Quaternion.identity); show point on clipping plane
        ResultAsText.instance.Add(U(x) + " " + U(y) + " "+name +" object marked");
        //Debug.Log(x+" "+y+" "+u(x) + " " + u(y) + " object marked");
        //sphere.transform.position = p;
    }
    private float U(float x)
    {
        float slope = ((umax - umin) / (xmax - xmin));
        float b = umin - slope * xmin;
        return slope * x + b;
    }
    private float V(float y)
    {
        float slope = ((vmax - vmin) / (ymax - ymin));
        float b = vmin - slope * ymin;
        return slope * y + b;
    }
}
