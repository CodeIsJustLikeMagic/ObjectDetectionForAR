using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultAsText : MonoBehaviour
{
    public static ResultAsText instance;
    public Text output;
    public Text labels;
    public Text markers;
    private void Awake()
    {
        instance = this;
    }

    public void Show(System.String json)
    {
        Debug.Log("show");
        output.text = json;
    }
    public void Add(System.String json)
    {
        output.text = output.text + "\n" + json;
    }
    public void ShowLabels(System.String json)
    {
        labels.text = json;
    }
    public void AddLabel(System.String json)
    {
        labels.text = labels.text + "\n" +json;
    }
    public void AddMarkers(System.String json)
    {
        markers.text = markers.text + "\n" + json;
    }
}
