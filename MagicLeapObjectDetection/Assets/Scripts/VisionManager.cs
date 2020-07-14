﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VisionManager : MonoBehaviour
{
    [System.Serializable]
    public class TagData
    {
        public string name;
        public float confidence;
    }

    [System.Serializable]
    public class AnalysedObject
    {
        public TagData[] tags;
        public string requestId;
        public object metadata;
    }
    public static VisionManager instance;

    // you must insert your service key here!    
    private string authorizationKey = ""; //Todo: this key is the opposite of secure
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string visionAnalysisEndpoint = "westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Tags";   // This is where you need to update your endpoint, if you set your location to something other than west-us.
    internal byte[] imageBytes;

    internal string imagePath;
    private void Awake()
    {
        Debug.Log("VisionManagerExists");
        // allows this instance to behave like a singleton
        instance = this;
        string path = "C:/ObjectDetectionForAR/ObjectDetectionForAR/Assets/Secure/authorizationKey.txt";
        StreamReader reader = new StreamReader(path);
        authorizationKey = reader.ReadToEnd();

        
    }

    public void processResponse(String jsonResponse)
    {
        Debug.Log("process Response");
        // The response will be in Json format
        // therefore it needs to be deserialized into the classes AnalysedObject and TagData
        AnalysedObject analysedObject = new AnalysedObject();
        analysedObject = JsonUtility.FromJson<AnalysedObject>(jsonResponse);

        if (analysedObject.tags == null)
        {
            Debug.Log("analysedObject.tagData is null");
        }
        else
        {
            Dictionary<string, float> tagsDictionary = new Dictionary<string, float>();

            foreach (TagData td in analysedObject.tags)
            {
                TagData tag = td as TagData;
                tagsDictionary.Add(tag.name, tag.confidence);
                Debug.Log(tag.name + " " + tag.confidence);
            }
            //ResultsLabel.instance.SetTagsToLastLabel(tagsDictionary);
            ResultAsText.instance.show();
        }
    }

    public IEnumerator AnalyseImage(byte[] imageBytes)
    {
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
        {
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
                string jsonResponse = null;
                jsonResponse = unityWebRequest.downloadHandler.text;
                Debug.Log(jsonResponse);

                processResponse(jsonResponse);
                //processResponse(jsonResponse)

            }
            catch (Exception exception)
            {
                Debug.Log("Json exception.Message: " + exception.Message);
            }

            yield return null;
        }
    }



    public IEnumerator AnalyseImageFromFile(string TestImagePath)
    {
        Debug.Log("VisionProcess Image from File");
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
        {
            // gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(TestImagePath);
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
                string jsonResponse = null;
                jsonResponse = unityWebRequest.downloadHandler.text;
                Debug.Log(jsonResponse);

                processResponse(jsonResponse);

            }
            catch (Exception exception)
            {
                Debug.Log("Json exception.Message: " + exception.Message);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Call the Computer Vision Service to submit the image.
    /// </summary>
    public IEnumerator AnalyseLastImageCaptured()
    {
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
        {
            // gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);
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
                string jsonResponse = null;
                jsonResponse = unityWebRequest.downloadHandler.text;

                // The response will be in Json format
                // therefore it needs to be deserialized into the classes AnalysedObject and TagData
                AnalysedObject analysedObject = new AnalysedObject();
                analysedObject = JsonUtility.FromJson<AnalysedObject>(jsonResponse);

                if (analysedObject.tags == null)
                {
                    Debug.Log("analysedObject.tagData is null");
                }
                else
                {
                    Dictionary<string, float> tagsDictionary = new Dictionary<string, float>();

                    foreach (TagData td in analysedObject.tags)
                    {
                        TagData tag = td as TagData;
                        tagsDictionary.Add(tag.name, tag.confidence);
                    }

                    ResultsLabel.instance.SetTagsToLastLabel(tagsDictionary);
                }
            }
            catch (Exception exception)
            {
                Debug.Log("Json exception.Message: " + exception.Message);
            }

            yield return null;
        }
    }
    /// <summary>
    /// Returns the contents of the specified file as a byte array.
    /// </summary>
    private static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        return binaryReader.ReadBytes((int)fileStream.Length);
    }
}