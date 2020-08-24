using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreater : MonoBehaviour
{
    public static LabelCreater instance;
    public GameObject markerprefab;
    private void Awake()
    {
        instance = this;
    }
    public List<string> labelsTexts = new List<string>();
    public List<GameObject> markers = new List<GameObject>();
    public void AddLabel(string str)
    {
        //ResultAsText.instance.AddLabel(str);
        labelsTexts.Add(str);
    }
    int m = 0;
    public void CreateMarker(Vector3 point, Vector3 normal, string text)
    {
        m = m + 1;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(markerprefab, point, rotation);
        MarkerBehavior b = go.GetComponent<MarkerBehavior>();
        if(b == null)
        {
            Debug.Log("no MarkerBehavior Component");
        }
        markers.Add(go);
        go.GetComponent<MarkerBehavior>().SetText(text);
        ShowLabels();
        ResultAsText.instance.AddMarkers("new marker " +m+" "+text);
    }
    private void ShowLabels()
    {
        string s = "";
        foreach(string l in labelsTexts)
        {
            s = s + "\n" + l;
        }
        ResultAsText.instance.ShowLabels(s);
    }
    private string Label(int m)
    {
        return labelsTexts[m];
    }
    private string FirstUnusedLabel()
    {
        //labels get put into the labelsText thing. Labels and markers should allways have the same number.
        //count how many markers we have. when instantiating a new one give it the label corresponding with the index.
        return labelsTexts[markers.Count - 1];
    }
    public void Update()
    {
        foreach(GameObject m in markers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
    }
}
