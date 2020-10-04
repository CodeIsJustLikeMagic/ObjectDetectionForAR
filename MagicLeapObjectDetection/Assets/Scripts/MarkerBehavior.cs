using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehavior : MonoBehaviour
{
    public TextMesh textHolder;
    public GameObject graysherePrefab;
    private List<GameObject> grayspheres = new List<GameObject>();
    private List<Vector3> points = new List<Vector3>();
    public GameObject mainMarker;
    public void Update()
    {
        mainMarker.transform.LookAt(Camera.main.transform.position);
    }
    public void SetText(string str)
    {
        textHolder.text = str;
        CreateGrayShere(this.transform.position);
    }
    public string GetText()
    {
        return textHolder.text;
    }

    private void CreateGrayShere(Vector3 point)
    {
        Debug.Log("created gray sphere at " + point);
        GameObject secondarymarker = Instantiate(graysherePrefab, point, Quaternion.identity, this.transform);
        grayspheres.Add(secondarymarker);
        points.Add(point);
    }

    public void UpdateLocation(Vector3 point)
    {
        //the same object has been detected again
        //change the position of the old label to midpoint
        //add gray shpere to indicate the two detected positions
        Vector3 sum = new Vector3(0, 0, 0);
        foreach(Vector3 v in points)
        {
            Debug.Log(v);
            sum = sum + v;
        }
        mainMarker.transform.position = (sum + point) / (points.Count + 1);
        for (int i = 0; i< points.Count; i++)
        {
            Debug.Log("move gray to " + point[i]);
            grayspheres[i].transform.position = points[i];
        }
        CreateGrayShere(point);
        Debug.Log("created gray sphere at " + point);
        Debug.Log("is alive");
    }



}
