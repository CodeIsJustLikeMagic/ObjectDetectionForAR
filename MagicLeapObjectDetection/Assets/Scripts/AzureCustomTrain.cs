using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AzureCustomTrain : MonoBehaviour
{
    //doesnt work and I gave up making it work because I dont have the time to invest in this.
    //Steps to make it work:
    //Upload Image
    //Add Regions (need to make a region Select Input)
    //Specify Tag (Input of new/old Tags)
    //Upload min 15 Images of one Object Klass
    //Make new Iteration and tell AzureCustomPrediction to use new Iteration
    public static AzureCustomTrain instance;
    private string trainingImageEndpoint = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.3/Training/projects/ac915246-5268-461f-bd11-cf0c1826d509/images";
    private string trainingKey = "";
    private string projectId = "ac915246-5268-461f-bd11-cf0c1826d509";
    public byte[] imageBytes;
    internal enum Tags { Mouse, Keyboard }
    
    private void Awake()
    {
        instance = this;
        TextAsset txt = (TextAsset)Resources.Load("trainingKey", typeof(TextAsset));
        trainingKey = txt.text +"blablA";
    }
    public IEnumerator UploadImage()
    {
        if(imageBytes == null)
        {
            InformationUI.instance.Add("no image saved to uplodad to train custom vision");
        }
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(trainingImageEndpoint, webForm))
        {
            unityWebRequest.SetRequestHeader("Content-Type", "application/cvtet-stream");
            unityWebRequest.SetRequestHeader("Training-Key", trainingKey);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);//imageBytes of image that was last taken;
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return unityWebRequest.SendWebRequest();
            long responseCode = unityWebRequest.responseCode;
            Debug.Log(responseCode);
            InformationUI.instance.Add("uploaded image, responsecode: "+responseCode);
            try
            {
                InformationUI.instance.Add(unityWebRequest.downloadHandler.text);
            }catch(Exception exception)
            {
                InformationUI.instance.Add("Json exception.Message: " + exception.Message);
            }
            yield return null;
        }
    }

    //public IEnumerator UpdateImageWithTags(string imageId, string tag, double left, double top, double width, double height)
    //{
    //    WWWForm webForm = new WWWForm();
    //    using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(trainingEndpoint, webForm))
    //    {
    //        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
    //        unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);
    //    }
    //}
    ///// <summary>
    ///// Call the Custom Vision Service to submit the image.
    ///// </summary>
    //public IEnumerator SubmitImageForTraining(byte[] image, string tag, double left, double top, double width, double height)
    //{
    //    yield return new WaitForSeconds(2);
    //    string imageId = string.Empty;
    //    string tagId = string.Empty;
    //    using (UnityWebRequest www = UnityWebRequest.Get(trainingEndpoint))
    //    {
    //        www.SetRequestHeader("Training-Key", trainingKey);
    //        www.downloadHandler = new DownloadHandlerBuffer();
    //        yield return www.SendWebRequest();
    //        string jsonResponse = www.downloadHandler.text;

    //        Tags_RootObject tagRootObject = JsonConvert.DeserializeObject<Tags_RootObject>(jsonResponse);

    //        foreach (TagOfProject tOP in tagRootObject.Tags)
    //        {
    //            if (tOP.Name == tag)
    //            {
    //                tagId = tOP.Id;
    //            }
    //        }
    //    }

    //    // Creating the image object to send for training
    //    List<IMultipartFormSection> multipartList = new List<IMultipartFormSection>();
    //    MultipartObject multipartObject = new MultipartObject();
    //    multipartObject.contentType = "application/octet-stream";
    //    multipartObject.fileName = "";
    //    multipartObject.sectionData = GetImageAsByteArray(imagePath);
    //    multipartList.Add(multipartObject);

    //    string createImageFromDataEndpoint = string.Format("{0}{1}/images?tagIds={2}", trainingEndpoint, projectId, tagId);

    //    using (UnityWebRequest www = UnityWebRequest.Post(createImageFromDataEndpoint, multipartList))
    //    {
    //        // Gets a byte array out of the saved image
    //        imageBytes = GetImageAsByteArray(imagePath);

    //        //unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
    //        www.SetRequestHeader("Training-Key", trainingKey);

    //        // The upload handler will help uploading the byte array with the request
    //        www.uploadHandler = new UploadHandlerRaw(imageBytes);

    //        // The download handler will help receiving the analysis from Azure
    //        www.downloadHandler = new DownloadHandlerBuffer();

    //        // Send the request
    //        yield return www.SendWebRequest();
    //        string jsonResponse = www.downloadHandler.text;
    //        ImageRootObject m = JsonConvert.DeserializeObject<ImageRootObject>(jsonResponse);
    //        imageId = m.Images[0].Image.Id;
    //    }
    //    StartCoroutine(TrainCustomVisionProject());
    //}
    ///// <summary>
    ///// Call the Custom Vision Service to train the Service.
    ///// It will generate a new Iteration in the Service
    ///// </summary>
    //public IEnumerator TrainCustomVisionProject()
    //{
    //    yield return new WaitForSeconds(2);

    //    WWWForm webForm = new WWWForm();

    //    string trainProjectEndpoint = string.Format("{0}{1}/train", trainingEndpoint, projectId);

    //    using (UnityWebRequest www = UnityWebRequest.Post(trainProjectEndpoint, webForm))
    //    {
    //        www.SetRequestHeader("Training-Key", trainingKey);
    //        www.downloadHandler = new DownloadHandlerBuffer();
    //        yield return www.SendWebRequest();
    //        string jsonResponse = www.downloadHandler.text;
    //        Debug.Log($"Training - JSON Response: {jsonResponse}");

    //        // A new iteration that has just been created and trained
    //        Iteration iteration = new Iteration();
    //        iteration = JsonConvert.DeserializeObject<Iteration>(jsonResponse);

    //        if (www.isDone)
    //        {
    //            trainingUI_TextMesh.text = "Custom Vision Trained";

    //            // Since the Service has a limited number of iterations available,
    //            // we need to set the last trained iteration as default
    //            // and delete all the iterations you dont need anymore
    //            StartCoroutine(SetDefaultIteration(iteration));
    //        }
    //    }
    //}
    ///// <summary>
    ///// Set the newly created iteration as Default
    ///// </summary>
    //private IEnumerator SetDefaultIteration(Iteration iteration)
    //{
    //    yield return new WaitForSeconds(5);
    //    trainingUI_TextMesh.text = "Setting default iteration";

    //    // Set the last trained iteration to default
    //    iteration.IsDefault = true;

    //    // Convert the iteration object as JSON
    //    string iterationAsJson = JsonConvert.SerializeObject(iteration);
    //    byte[] bytes = Encoding.UTF8.GetBytes(iterationAsJson);

    //    string setDefaultIterationEndpoint = string.Format("{0}{1}/iterations/{2}",
    //                                                    trainingEndpoint, projectId, iteration.Id);

    //    using (UnityWebRequest www = UnityWebRequest.Put(setDefaultIterationEndpoint, bytes))
    //    {
    //        www.method = "PATCH";
    //        www.SetRequestHeader("Training-Key", trainingKey);
    //        www.SetRequestHeader("Content-Type", "application/json");
    //        www.downloadHandler = new DownloadHandlerBuffer();

    //        yield return www.SendWebRequest();

    //        string jsonResponse = www.downloadHandler.text;

    //        if (www.isDone)
    //        {
    //            trainingUI_TextMesh.text = "Default iteration is set \nDeleting Unused Iteration";
    //            StartCoroutine(DeletePreviousIteration(iteration));
    //        }
    //    }
    //}
    ///// <summary>
    ///// Delete the previous non-default iteration.
    ///// </summary>
    //public IEnumerator DeletePreviousIteration(Iteration iteration)
    //{
    //    yield return new WaitForSeconds(5);
        

    //    string iterationToDeleteId = string.Empty;

    //    string findAllIterationsEndpoint = string.Format("{0}{1}/iterations", trainingEndpoint, projectId);

    //    using (UnityWebRequest www = UnityWebRequest.Get(findAllIterationsEndpoint))
    //    {
    //        www.SetRequestHeader("Training-Key", trainingKey);
    //        www.downloadHandler = new DownloadHandlerBuffer();
    //        yield return www.SendWebRequest();

    //        string jsonResponse = www.downloadHandler.text;

    //        // The iteration that has just been trained
    //        List<Iteration> iterationsList = new List<Iteration>();
    //        iterationsList = JsonConvert.DeserializeObject<List<Iteration>>(jsonResponse);

    //        foreach (Iteration i in iterationsList)
    //        {
    //            if (i.IsDefault != true)
    //            {
    //                Debug.Log($"Cleaning - Deleting iteration: {i.Name}, {i.Id}");
    //                iterationToDeleteId = i.Id;
    //                break;
    //            }
    //        }
    //    }

    //    string deleteEndpoint = string.Format("{0}{1}/iterations/{2}", trainingEndpoint, projectId, iterationToDeleteId);

    //    using (UnityWebRequest www2 = UnityWebRequest.Delete(deleteEndpoint))
    //    {
    //        www2.SetRequestHeader("Training-Key", trainingKey);
    //        www2.downloadHandler = new DownloadHandlerBuffer();
    //        yield return www2.SendWebRequest();
    //        string jsonResponse = www2.downloadHandler.text;

    //        trainingUI_TextMesh.text = "Iteration Deleted";
    //        yield return new WaitForSeconds(2);
    //        trainingUI_TextMesh.text = "Ready for next \ncapture";

    //        yield return new WaitForSeconds(2);
    //        trainingUI_TextMesh.text = "";
    //        ImageCapture.Instance.ResetImageCapture();
    //    }
    //}
    ///// <summary>
    ///// Returns the contents of the specified image file as a byte array.
    ///// </summary>
    //static byte[] GetImageAsByteArray(string imageFilePath)
    //{
    //    FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
    //    BinaryReader binaryReader = new BinaryReader(fileStream);
    //    return binaryReader.ReadBytes((int)fileStream.Length);
    //}
}
