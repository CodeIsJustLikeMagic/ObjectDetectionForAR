using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AzureCustomPrediction : MonoBehaviour
{

    public static AzureCustomPrediction instance;

    private string predictionKey = "";
    private string predictionEndpointStart = "https://detectiontrainingforar.cognitiveservices.azure.com/customvision/v3.0/Prediction/ac915246-5268-461f-bd11-cf0c1826d509/detect/iterations/";
    private string predictionEndpoint = "";
    [HideInInspector] public byte[] imageBytes;
    public string iteration = "Iteration4";
    private void Awake()
    {
        instance = this;
        TextAsset txt = (TextAsset)Resources.Load("predictionKey", typeof(TextAsset));
        predictionKey = txt.text;
        predictionEndpoint = predictionEndpointStart + iteration + "/image";
        InformationUI.instance.ShowIteration(iteration);
    }

    public IEnumerator AnalyseImage(byte[] imageBytes, SavedCameraState cameraState)
    {
        //ResultAsText.instance.Show(authorizationKey);
        InformationUI.instance.Show("AnalyseImage");
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(predictionEndpoint, webForm))
        {
            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request
            yield return unityWebRequest.SendWebRequest();
            long responseCode = unityWebRequest.responseCode;
            Debug.Log(responseCode);
            try
            {
                InformationUI.instance.Add("responseCode " + responseCode);
                string jsonResponse = null;
                jsonResponse = unityWebRequest.downloadHandler.text;
                Debug.Log(jsonResponse);
                HandleJson(imageBytes, jsonResponse, cameraState);
            }
            catch (Exception exception)
            {
                InformationUI.instance.Show("Json exception.Message: " + exception.Message);
                Debug.Log("Json exception.Message: " + exception.Message);
            }
            yield return null;
        }
    }

    [System.Serializable]
    public class Box
    {
        public double left;
        public double top;
        public double width;
        public double height;
    }
    [System.Serializable]
    public class DetectedObject
    {
        public double probability;
        public string tagId;
        public string tagName;
        public Box boundingBox;
    }
    [System.Serializable]
    public class DetectionResponse
    {
        public string id;
        public string project;
        public string iteration;
        public string created;
        public DetectedObject[] predictions;
    }
    public void HandleJson(byte[] imageBytes, System.String jsonResponse, SavedCameraState cpos)
    {
        DetectionResponse det = new DetectionResponse();
        det = JsonUtility.FromJson<DetectionResponse>(jsonResponse);
        InformationUI.instance.Add("Custom Prediction Handle Json");
        Texture2D texture = new Texture2D(8, 8);
        texture.LoadImage(imageBytes);
        Debug.Log(texture.width + " " + texture.height);
        foreach (DetectedObject obj in det.predictions)
        {
            if(obj.probability >= 0.8)
            {
                Debug.Log(obj.tagName);
                int u = (int)(texture.width * obj.boundingBox.left);
                int v = (int)(texture.height * obj.boundingBox.top);
                int w = (int)(texture.width * obj.boundingBox.width);
                int h = (int)(texture.height * obj.boundingBox.height);
                u = u + w / 2;
                v = v + h / 2;
                InformationUI.instance.Add(obj.tagName + " " + u + " " + v +" "+obj.probability);
                PixelToWorld.instance.Cast(u, v, cpos, obj.tagName, 2); //make it use second material to differentiate between Object Detection and Custom Vision
            }
        }
    }
}
