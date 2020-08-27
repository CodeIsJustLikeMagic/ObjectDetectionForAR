using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedCameraState
{
    public Matrix4x4 cameratoWorldMatrix;
    public Transform ctransform;
    public SavedCameraState(Camera c)
    {
        cameratoWorldMatrix = c.cameraToWorldMatrix;
        ctransform = c.transform;
    }
}
