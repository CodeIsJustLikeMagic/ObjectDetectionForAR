using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreater : MonoBehaviour
{
    public static LabelCreater instance;
    public GameObject markerprefab;
    public GameObject markerprefab2;
    private void Awake()
    {
        instance = this;
    }
    public List<GameObject> markers = new List<GameObject>();
    public void CreateMarker(Vector3 point, Vector3 normal, string text, int material)
    {
        GameObject prefab = markerprefab;
        if(material == 2)
        {
            markerprefab = markerprefab2;
        }
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(markerprefab, point, rotation);
        MarkerBehavior b = go.GetComponent<MarkerBehavior>();
        if(b == null)
        {
            Debug.Log("no MarkerBehavior Component");
        }
        markers.Add(go);
        go.GetComponent<MarkerBehavior>().SetText(text);
        ResultAsText.instance.AddMarkers("new marker "+text);
    }
    public void Update()
    {
        foreach(GameObject m in markers)
        {
            m.transform.LookAt(Camera.main.transform.position);
        }
    }
}
