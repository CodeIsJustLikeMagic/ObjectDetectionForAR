using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class AzureObjectDetection : MonoBehaviour
{
    public static AzureObjectDetection instance;

    // you must insert your service key here!    
    private string authorizationKey = ""; //Todo: this key is the opposite of secure
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string visionAnalysisEndpoint = "westeurope.api.cognitive.microsoft.com/vision/v2.0/detect";   // This is where you need to update your endpoint, if you set your location to something other than west-us.
    
    private void Awake()
    {
        instance = this;
        TextAsset txt = (TextAsset)Resources.Load("authorizationKey", typeof(TextAsset));
        authorizationKey = txt.text;
    }

    public IEnumerator AnalyseImage(byte[] imageBytes, SavedCameraState cameraState)
    {
        //ResultAsText.instance.Show(authorizationKey);
        ResultAsText.instance.Show("AnalyseImage");
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
        {
            ResultAsText.instance.Add("made a UnityWebRequest thing");
            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader(ocpApimSubscriptionKeyHeader, authorizationKey);
            // the download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            // the upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";
            yield return unityWebRequest.SendWebRequest();

            long responseCode = unityWebRequest.responseCode;
            try
            {
                ResultAsText.instance.Add("responseCode " + responseCode);
                string jsonResponse = null;
                jsonResponse = unityWebRequest.downloadHandler.text;
                Debug.Log(jsonResponse);
                ResultAsText.instance.Add(jsonResponse);
                HandleJsonResponse(jsonResponse, cameraState);
            }
            catch (Exception exception)
            {
                ResultAsText.instance.Show("Json exception.Message: " + exception.Message);
                Debug.Log("Json exception.Message: " + exception.Message);
            }
            yield return null;
        }
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
    public void HandleJsonResponse(System.String jsonResponse, SavedCameraState cpos)
    {
        jsonResponse = jsonResponse.Replace("object", "objectName"); //c# dosn't like "public string object"
        DetectionResponse det = new DetectionResponse();
        det = JsonUtility.FromJson<DetectionResponse>(jsonResponse);
        ResultAsText.instance.Show(" Handle Json");
        foreach (DetectedObject obj in det.objectNames)
        {
            Debug.Log(obj.objectName);
            int x = obj.rectangle.x + (obj.rectangle.w / 2);
            int y = obj.rectangle.y + (obj.rectangle.h / 2);
            ResultAsText.instance.Add(obj.objectName);
            PixelToWorld.instance.Cast(x, y, cpos, obj.objectName);
        }
    }
}
