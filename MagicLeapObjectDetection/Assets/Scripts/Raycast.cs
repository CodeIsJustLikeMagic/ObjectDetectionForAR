using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

//https://developer.magicleap.com/en-us/learn/guides/raycast-in-unity
public class Raycast : MonoBehaviour
{
    public static Raycast instance;
    public Transform ctransform;
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
    public MLRaycast.QueryParams CreateRaycastParams()
    {
        MLRaycast.QueryParams _raycastParams = new MLRaycast.QueryParams
        {
            // Update the parameters with our Camera's transform
            Position = ctransform.position,
            Direction = ctransform.forward,
            UpVector = ctransform.up,
            // Provide a size of our raycasting array (1x1)
            Width = 1,
            Height = 1
        };
        return _raycastParams;
    }
    // Update is called once per frame
    void Update()
    {
        // Create a raycast parameters variable
        MLRaycast.QueryParams _raycastParams = CreateRaycastParams();
        // Feed our modified raycast parameters and handler to our raycast request
        MLRaycast.Raycast(_raycastParams, HandleOnReceiveRaycast);
    }
    private IEnumerator NormalMarker(Vector3 point, Vector3 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(prefab, point, rotation);
        yield return new WaitForSeconds(2);
        Destroy(go);
    }
    void HandleOnReceiveRaycast(MLRaycast.ResultState state, UnityEngine.Vector3 point, Vector3 normal, float confidence)
    {
        if (state == MLRaycast.ResultState.HitObserved)
        {
            StartCoroutine(NormalMarker(point, normal));
        }
    }
    private void OnDestroy()
    {
        MLRaycast.Stop();
    }
}
