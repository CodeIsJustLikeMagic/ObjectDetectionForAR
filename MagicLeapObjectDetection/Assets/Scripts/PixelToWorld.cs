using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PixelToWorld : MonoBehaviour
{
    public static PixelToWorld instance;
    void Awake()
    {
        instance = this;
    }
    
    public GameObject bluespherePrefab;
    private float xmin = 0;//525;
    private float xmax = 1920;//1585;//1920;
    private float ymin = 0;//150;
    private float ymax = 1080;//935;//1080;

    private float umin = -0.295F;
    private float umax = 0.2281F;
    private float vmin = 0.1546F;
    private float vmax = -0.1507F;
    public void markEdges(Matrix4x4 m)
    {
        markEdges(new SavedCameraState(Camera.main));
    }
    public void markEdges(SavedCameraState cpos)
    {
        Cast(xmin, ymin, cpos,"edge up left");
        Cast(xmin, ymax, cpos,"edge down left");
        Cast(xmax, ymin, cpos,"edge up right");
        Cast(xmax, ymax, cpos,"edge down right");
        //Instantiate(bluespherePrefab, m.MultiplyPoint(new Vector3(-0.15F, -0.1F, -0.4F)), Quaternion.identity);
    }
    public void Cast(float x, float y, SavedCameraState cpos, string name)
    {
        Cast(x, y, cpos, name, 0);
    }
    public void Cast(float x, float y,SavedCameraState cpos, string name, int material)
    {
        Cast(x, y, cpos, bluespherePrefab, name, false, material);
    }
    public void Cast(float x, float y, SavedCameraState cpos,GameObject clippingPlaneMarker, string name, bool showClippingPlane, int material)
    {
        //move pointer basen on photo pixel position (1080/1920)
        Vector3 p = cpos.cameratoWorldMatrix.MultiplyPoint(new Vector3(U(x), V(y), -0.4F));
        Raycast.instance.StartCast(Raycast.instance.CreateRaycastParams(cpos.ctransform, p), name, material);
        if (showClippingPlane)
        {
            GameObject sphere2 = Instantiate(clippingPlaneMarker, p, Quaternion.identity);// show point on clipping plane
        }
        ResultAsText.instance.Add(x + " " + y + " " + U(x) + " " + U(y) + " " + name + " object marked");
    }
    private float U(float x)
    {
        float slope = ((umax - umin) / (xmax - xmin));
        float b = umin - slope * xmin;
        return slope * x + b;
    }
    private float V(float y)
    {
        float slope = ((vmax - vmin) / (ymax - ymin));
        float b = vmin - slope * ymin;
        return slope * y + b;
    }
}