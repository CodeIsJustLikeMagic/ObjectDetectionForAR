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
        GameObject marker = Instantiate(prefab, point, rotation);
        MarkerBehavior b = marker.GetComponent<MarkerBehavior>();
        if(b == null)
        {
            Debug.Log("no MarkerBehavior Component");
        }
        if(material == 2)
        {
            custompredictionMarkers.Add(marker);
        }
        else
        {
            objectdetectionMarkers.Add(marker);
        }
        marker.GetComponent<MarkerBehavior>().SetText(text);
        markersOnCanvas();
    }
    
    public void Update()
    {//roate existing Markers towars camera
        foreach (GameObject m in objectdetectionMarkers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
        foreach (GameObject m in custompredictionMarkers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
       
    }

    //updates UI Display of which Markers exist in the world
    private void markersOnCanvas()
    {
        string str = "";
        foreach (GameObject m in objectdetectionMarkers)
        {
            str = str + m.GetComponent<MarkerBehavior>().GetText() +"\n";
        }
        str = str + "Custom Vision: \n";
        foreach (GameObject m in custompredictionMarkers)
        {
            str = str + m.GetComponent<MarkerBehavior>().GetText()+"\n";
        }
        InformationUI.instance.ShowMarkers(str);
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

    public void EraseLast()
    {
        if(showState == 0)//erase last blue, erase red if no blue exists
        {
            if (!RemoveFrom(custompredictionMarkers))
            {
                RemoveFrom(objectdetectionMarkers);
            }
        }else if(showState == 1)//erase last red
        {
            RemoveFrom(objectdetectionMarkers);
        }else if(showState == 2)//erase last blue
        {
            RemoveFrom(custompredictionMarkers);
        }
        markersOnCanvas();
    }
    
    private bool RemoveFrom(List<GameObject> l)
    {
        if (l.Count > 0) //prevent IndexOutOfRangeException for empty list
        {
            Object.Destroy(l[l.Count - 1]);
            l.RemoveAt(l.Count - 1);
            return true;
        }
        return false;
    }

}
