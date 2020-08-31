using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreater : MonoBehaviour
{
    public static LabelCreater instance;
    public GameObject MarkerObjectDetection;
    public GameObject MarkerCustomPrediction;
    private void Awake()
    {
        instance = this;
    }
    public List<GameObject> objectdetectionMarkers = new List<GameObject>();
    public List<GameObject> custompredictionMarkers = new List<GameObject>();
    public void CreateMarker(Vector3 point, Vector3 normal, string text, int material)
    {
        GameObject prefab = MarkerObjectDetection;
        if(material == 2)
        {
            prefab = MarkerCustomPrediction;
        }
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(prefab, point, rotation);
        MarkerBehavior b = go.GetComponent<MarkerBehavior>();
        if(b == null)
        {
            Debug.Log("no MarkerBehavior Component");
        }
        if(material == 2)
        {
            custompredictionMarkers.Add(go);
        }
        else
        {
            objectdetectionMarkers.Add(go);
        }
        go.GetComponent<MarkerBehavior>().SetText(text);
        ResultAsText.instance.AddMarkers(text);
    }
    public void Update()
    {
        foreach(GameObject m in objectdetectionMarkers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
        foreach (GameObject m in custompredictionMarkers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
    }
    int showState = 0;
    public void ToggleShow()
    {
        showState = (showState + 1) % 3 ;
        if(showState == 0)//showAll
        {
            showorhide(true, true);
        }
        else if(showState == 1)//only red
        {
            showorhide(true, false);
        }
        else if(showState == 2)//only blue
        {
            showorhide(false, true);
        }
    }

    private void showorhide(bool showred, bool showblue)
    {
        foreach (GameObject m in objectdetectionMarkers)//only red markers
        {
            m.SetActive(showred);
        }
        foreach (GameObject m in custompredictionMarkers)
        {
            m.SetActive(showblue);
        }
    }
}
