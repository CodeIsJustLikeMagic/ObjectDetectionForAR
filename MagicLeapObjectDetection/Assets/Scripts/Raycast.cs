using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

//https://developer.magicleap.com/en-us/learn/guides/raycast-in-unity
public class Raycast : MonoBehaviour
{
    public static Raycast instance;
    //public Transform ctransform;
    public GameObject prefab;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        MLRaycast.Start();
    }
    public MLRaycast.QueryParams CreateRaycastParams(Transform ctransform, Vector3 target)
    {
        MLRaycast.QueryParams _raycastParams = new MLRaycast.QueryParams
        {
            // Update the parameters with our Camera's transform
            Position = ctransform.position,
            Direction = target - ctransform.position,
            //Direction = ctransform.forward,
            UpVector = ctransform.up,
            // Provide a size of our raycasting array (1x1)
            Width = 1,
            Height = 1
        };
        return _raycastParams;
    }
    // Update is called once per frame
    public void StartCast(MLRaycast.QueryParams raycastParams, string text, int material)
    {
        Debug.Log("Raycast started");
        MLRaycast.Raycast(raycastParams, (state, point, normal, confidence) => HandleOnReceiveRaycast(text, material, state, point, normal, confidence));
    }
    void HandleOnReceiveRaycast(string text, int material, MLRaycast.ResultState state, UnityEngine.Vector3 point, Vector3 normal, float confidence)
    {
        Debug.Log("Finished Raycast");
        if (state == MLRaycast.ResultState.HitObserved)
        {
            Debug.Log("Hit something, want to mark it");
            Debug.Log("normal" + normal);
            LabelCreater.instance.CreateMarker(point, new Vector3(0,0,0), text, material);
        }
    }
    private void OnDestroy()
    {
        MLRaycast.Stop();
    }
}
