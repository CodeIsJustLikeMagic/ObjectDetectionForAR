using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    public static SphereTest instance;
    void Awake()
    {
        instance = this;
    }
    public GameObject sphere;
    Vector3 p;

    void Update()//move sphere to somewhere within the camera view
    {
        Matrix4x4 m = Camera.main.cameraToWorldMatrix;
        p = m.MultiplyPoint(new Vector3(0, 0, -0.4F));
        sphere.transform.position = p;
    }

    public void ShowPos()
    {
        ResultAsText.instance.Show(p.ToString());
    }
}
