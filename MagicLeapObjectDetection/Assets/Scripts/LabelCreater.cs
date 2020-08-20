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
        labelsTexts.Add(str);
    }
    public void CreateMarker(Vector3 point, Vector3 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(markerprefab, point, rotation);
        MarkerBehavior b = go.GetComponent<MarkerBehavior>();
        if(b == null)
        {
            Debug.Log("no MarkerBehavior Component");
        }
        markers.Add(go);
        go.GetComponent<MarkerBehavior>().SetText(FirstUnusedLabel());
    }
    private string FirstUnusedLabel()
    {
        //labels get put into the labelsText thing. Labels and markers should allways have the same number.
        //count how many markers we have. when instantiating a new one give it the label corresponding with the index.
        return labelsTexts[markers.Count - 1];
    }
}
