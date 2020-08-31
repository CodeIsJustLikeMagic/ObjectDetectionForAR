using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisplay : MonoBehaviour
{
    public static MoveDisplay instance;
    public GameObject display;
    private void Awake()
    {
        instance = this;
    }
    public void ReorientCanvas()
    {
        Matrix4x4 m = Camera.main.cameraToWorldMatrix;
        display.transform.position = m.MultiplyPoint(new Vector3(0, 0, -1F));
        display.transform.LookAt(Camera.main.transform.position);
    }
}
