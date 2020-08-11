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
        p = m.MultiplyPoint(new Vector3(-0.29F, 0.216F, -0.3803704F));
        sphere.transform.position = p;
        HandleResult.instance.cast(600, 600, m);

    }

    public void ShowPos()
    {
        ResultAsText.instance.Show(p.ToString());
        
    }
}
