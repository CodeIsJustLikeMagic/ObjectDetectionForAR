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

    public void markEdges(Matrix4x4 m)
    {
        markEdges(new SavedCameraState(Camera.main));
    }
    public void markEdges(SavedCameraState cpos)
    {
        Cast(umin, vmin, cpos,"edge up left");
        Cast(umin, vmax, cpos,"edge down left");
        Cast(umax, vmin, cpos,"edge up right");
        Cast(umax, vmax, cpos,"edge down right");
        //Instantiate(bluespherePrefab, m.MultiplyPoint(new Vector3(-0.15F, -0.1F, -0.4F)), Quaternion.identity);
    }
    public void Cast(float u, float v, SavedCameraState cpos,GameObject clippingPlaneMarker, 
        string objectName, bool showClippingPlane, int material)
    {
        //scale v,v to x,y range
        Vector3 offset = new Vector3(X(u), Y(v), -0.4F);
        Vector3 p = cpos.cameratoWorldMatrix.MultiplyPoint(offset);
        Raycast.instance.StartCast(Raycast.instance.CreateRaycastParams(cpos.ctransform, p), objectName, material);
        if (showClippingPlane)
        {
            GameObject sphere2 = Instantiate(clippingPlaneMarker, p, Quaternion.identity);// show point on clipping plane
        }
        //InformationUI.instance.Add(u + " " + v + " " + X(u) + " " + X(v) + " " + objectName + " object marked");
    }
    //Picture u and v ranges
    private float umin = 0;//left
    private float umax = 1920;//right
    private float vmin = 0;//up
    private float vmax = 1080;//down
    //Offset Vektor x and y ranges
    private float xmin = -0.295F;//left
    private float xmax = 0.2281F;//right
    private float ymin = 0.1546F;//up
    private float ymax = -0.1507F;//down
    private float X(float u)
    {
        float slope = ((xmax - xmin) / (umax - umin));
        float b = xmin - slope * umin;
        return slope * u + b;
    }
    private float Y(float v)
    {
        float slope = ((ymax - ymin) / (vmax - vmin));
        float b = ymin - slope * vmin;
        return slope * v + b;
    }

    public void Cast(float u, float v, SavedCameraState cpos, string name)
    {
        Cast(u, v, cpos, name, 0);
    }
    public void Cast(float u, float v, SavedCameraState cpos, string name, int material)
    {
        Cast(u, v, cpos, bluespherePrefab, name, false, material);
    }
}