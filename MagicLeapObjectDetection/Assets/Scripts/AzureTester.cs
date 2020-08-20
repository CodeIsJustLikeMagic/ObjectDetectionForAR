using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AzureTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            Debug.Log("testing image");
            string filename = "C:/ObjectDetectionForAR/MagicLeapObjectDetection/Assets/TestImages/IMG_20200702_153228.jpg";
            
            //string.Format(@"CapturedImage{0}.jpg", tapsCount);

            string filePath = Path.Combine(Application.persistentDataPath, filename);
            Debug.Log(filePath);
            byte[] imageBytes = GetImageAsByteArray(filePath);
            StartCoroutine(VisionManager.instance.AnalyseImage(imageBytes, new CameraPosition(Camera.main)));
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
