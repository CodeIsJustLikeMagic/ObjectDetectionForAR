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
        display.transform.LookAt(Camera.main.transform.position);
    }
    private float umin = -0.295F;//0.29F;
    private float umax = 0.2281F;
    private float vmin = 0.1546F;//-0.216F;
    private float vmax = -0.1507F;
    public void Runtest()
    {
        //Vector3 p = Camera.main.cameraToWorldMatrix.MultiplyPoint(new Vector3(umin, vmax, -0.4F));
        //GameObject sphere2 = Instantiate(sphere, p, Quaternion.identity);
        HandleResult.instance.markEdges(Camera.main.cameraToWorldMatrix);
        //LabelCreater.instance.CreateMarker(new Vector3(0, 0, 0), new Vector3(0, 0, 0), "test");
    }

}
