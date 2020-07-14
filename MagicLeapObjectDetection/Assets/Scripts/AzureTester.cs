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
            StartCoroutine(VisionManager.instance.AnalyseImageFromFile(filename));
        }
    }
}
