using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestDetection : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            TestCustomPrediction();
            //TestAzureDetection();
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

    private void TestAzureDetection()
    {
        Debug.Log("testing regular detection");
        string filename = "C:/ObjectDetectionForAR/MagicLeapObjectDetection/Assets/TestImages/IMG_20200702_153228.jpg";

        //string.Format(@"CapturedImage{0}.jpg", tapsCount);

        string filePath = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(filePath);
        byte[] imageBytes = GetImageAsByteArray(filePath);
        StartCoroutine(AzureObjectDetection.instance.AnalyseImage(imageBytes, new SavedCameraState(Camera.main)));
    }

    private void TestCustomPrediction()
    {
        Debug.Log("testing custom prediction");
        string filename = "C:/ObjectDetectionForAR/MagicLeapObjectDetection/Assets/TestImages/IMG_20200828_152817.jpg";
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(filePath);
        byte[] imageBytes = GetImageAsByteArray(filePath);
        StartCoroutine(AzureCustomPrediction.instance.AnalyseImage(imageBytes, new SavedCameraState(Camera.main)));


        MySingletonClass.instance.DoSomething();


    }
}
