using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    public GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()//move sphere to somewhere within the camera view
    {
        Matrix4x4 m = Camera.main.cameraToWorldMatrix;
        Vector3 p = m.MultiplyPoint(new Vector3(1.0F, 0, -1.0F));
        sphere.transform.position = p;
    }
}
