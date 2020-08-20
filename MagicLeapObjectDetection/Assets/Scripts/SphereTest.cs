using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    public GameObject display;
    public static SphereTest instance;
    void Awake()
    {
        instance = this;
    }
    public GameObject sphere;
    Vector3 p;

    void Update()//move sphere to somewhere within the camera view
    {
        if (Input.GetKeyDown("f"))
        {
            Runtest();
        }
        if (Input.GetKeyDown("r"))
        {
            ReorientCanvas();
        }
    }

    public void ShowPos()
    {
        ReorientCanvas();
        //ResultAsText.instance.Show(p.ToString());
        //HandleResult.instance.markEdges(Camera.main.cameraToWorldMatrix);
    }

    public void ReorientCanvas()
    {
        Matrix4x4 m = Camera.main.cameraToWorldMatrix;
        p = m.MultiplyPoint(new Vector3(0, 0, -1F));
        display.transform.position = p;
    }

    public void Runtest()
    {
        LabelCreater.instance.CreateMarker(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }
}
