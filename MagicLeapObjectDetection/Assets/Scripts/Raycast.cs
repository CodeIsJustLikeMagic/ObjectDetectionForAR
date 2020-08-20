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
    void Update()
    {
        //HandleResult.instance.markEdges(Camera.main.cameraToWorldMatrix);
    }
    public void StartCast(MLRaycast.QueryParams raycastParams)
    {
        Debug.Log("Raycast started");
        MLRaycast.Raycast(raycastParams, HandleOnReceiveRaycast);
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
        Debug.Log("Finished Raycast");
        if (state == MLRaycast.ResultState.HitObserved)
        {
            Debug.Log("Hit something, want to mark it");
            //StartCoroutine(NormalMarker(point, normal));
            //PermanentMarker(point, normal);
            Debug.Log("normal" + normal);
            LabelCreater.instance.CreateMarker(point, new Vector3(0,0,0));
        }
    }
    private void PermanentMarker(Vector3 point, Vector3 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject go = Instantiate(prefab, point, rotation);
    }
    private void OnDestroy()
    {
        MLRaycast.Stop();
    }
}
