using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition
{
    public Matrix4x4 cameratoWorldMatrix;
    public Transform ctransform;
    public CameraPosition(Camera c)
    {
        cameratoWorldMatrix = c.cameraToWorldMatrix;
        ctransform = c.transform;
    }
}
