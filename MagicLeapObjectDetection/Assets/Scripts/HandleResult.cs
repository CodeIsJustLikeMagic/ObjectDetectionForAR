using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleResult : MonoBehaviour
{
    public Transform positionIndicator;
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

    public void HandleJson(byte[] imageBytes, System.String jsonResponse, Matrix4x4 cameraToWorldMatrix)
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
            Debug.Log("Creating labelPrefab");
            Vector3 p = cameraToWorldMatrix.MultiplyPoint(new Vector3(x, y, 0));
            Transform l = Instantiate(positionIndicator, p, Quaternion.identity);
            Debug.Log("Creating Done");
            ResultAsText.instance.Add(p.ToString() + " " + obj.objectName);
            labelsTexts.Add(obj.objectName);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(p, 0.2F);
        }
        Convert(imageBytes);
    }

    public void Convert(byte[] imageData)
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
}
