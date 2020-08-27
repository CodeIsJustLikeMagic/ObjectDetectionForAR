using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TrainVision : MonoBehaviour
{
    //https://docs.microsoft.com/en-us/azure/cognitive-services/Custom-Vision-Service/quickstarts/image-classification?pivots=programming-language-csharp
    public TrainVision instance;
    // you must insert your service key here!    
    private string trainingKey = ""; //Todo: this key is the opposite of secure
    private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string visionAnalysisEndpoint = "westeurope.api.cognitive.microsoft.com/vision/v2.0/detect";   // This is where you need to update your endpoint, if you set your location to something other than west-us.
    internal byte[] imageBytes;

    internal string imagePath;
    private void Awake()
    {
        Debug.Log("VisionManagerExists");
        instance = this;
        TextAsset txt = (TextAsset)Resources.Load("trainingKey", typeof(TextAsset));
        trainingKey = txt.text;
    }
}
