using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLabels : MonoBehaviour
{
    public GameObject display;
    public static TestLabels instance;
    void Awake()
    {
        instance = this;
    }
    public GameObject sphere;

    void Update()//move sphere to somewhere within the camera view
    {
        if (Input.GetKeyDown("w"))
        {
            Runtest();
        }
        if (Input.GetKeyDown("e"))
        {
            MoveDisplay.instance.ReorientCanvas();
        }
    }
    
    private float umin = -0.295F;//0.29F;
    private float umax = 0.2281F;
    private float vmin = 0.1546F;//-0.216F;
    private float vmax = -0.1507F;
    public void Runtest()
    {
        //PixelToWorld.instance.markEdges(Camera.main.cameraToWorldMatrix);
        LabelCreater.instance.CreateMarker(new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), "test", 1);
        LabelCreater.instance.CreateMarker(new Vector3(0, 0.03f, 0), new Vector3(0, 0, 0), "test", 1);
        LabelCreater.instance.CreateMarker(new Vector3(0.03f, 0.0f, 0), new Vector3(0, 0, 0), "test", 1);
        LabelCreater.instance.CreateMarker(new Vector3(-0.03f, 0.0f, 0), new Vector3(0, 0, 0), "test", 1);
        //LabelCreater.instance.EraseLast();
        //Vector3 p = Camera.main.cameraToWorldMatrix.MultiplyPoint(new Vector3(umin, vmax, -0.4F));
        //GameObject sphere2 = Instantiate(sphere, p, Quaternion.identity);

    }
}
