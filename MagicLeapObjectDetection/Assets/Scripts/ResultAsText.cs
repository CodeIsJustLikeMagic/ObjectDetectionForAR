using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultAsText : MonoBehaviour
{
    public static ResultAsText instance;
    public Text output;
    public Text markers;
    public Text iteration;
    private void Awake()
    {
        instance = this;
    }

    public void Show(System.String json)
    {
        output.text = json;
    }
    public void Add(System.String json)
    {
        output.text = output.text + "\n" + json;
    }
    public void AddMarkers(System.String json)
    {
        markers.text = markers.text + "\n" + json;
    }
    public void ShowIteration(string str)
    {
        iteration.text = "Custom Vision Iteration : " + str;
    }
    public void ShowMarkers(string str)
    {
        markers.text = "Markers :\n" + str;
    }
}
