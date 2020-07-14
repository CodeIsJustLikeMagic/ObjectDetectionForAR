using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using System.Threading;
using MagicLeap.Core.StarterKit;

public class TakePicture : MonoBehaviour
{
    // Start is called before the first frame update
    private MLInput.Controller _controller;
    private MLPrivilegeRequesterBehavior _privRequester = null;
    private bool _granted = false;
    [SerializeField, Tooltip("Object to set new images on.")]
    private GameObject _previewObject = null;
    

    private object _cameraLockObject = new object();

    private bool _isCameraConnected = false;

    private bool _isCapturing = false;

    private bool _hasStarted = false;
   
    private Thread _captureThread = null;
    
    void Awake()
    {
        //get privileges
        _privRequester = GetComponent<MLPrivilegeRequesterBehavior>();
        _privRequester.OnPrivilegesDone += HandlePrivilegesDone;
    }

    void HandlePrivilegesDone(MLResult result)
    {
        if (!result.IsOk)
        {
            Debug.Log("we didnt get privileges");
        }
        Debug.Log("we got privileges");
        _granted = true;
        StartCapture(); //enables camera and callbacks
    }

    void OnDestroy()
    {
        //clean up privileges
        if (_privRequester != null)
        {
            _privRequester.OnPrivilegesDone -= HandlePrivilegesDone;
        }
        //clean up input
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
        //clean up camera use
        lock (_cameraLockObject)
        {
            if (_isCameraConnected)
            {
                MLCamera.OnRawImageAvailable -= OnCaptureRawImageComplete;

                _isCapturing = false;
                DisableMLCamera();
            }
        }
    }
    #region input
    void Start()
    {
        //get user input. we are using bumper and trigger
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
        if(_controller == null)
        {
            Debug.Log("Controller not found. Did you start Lab?");
            enabled = false;
        } 
        MLInput.OnControllerButtonUp += OnButtonUp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTrigger();
    }

    bool pressed = false;

    void OnButtonUp(byte controllerId, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.Bumper)
        {
            Debug.Log("bumper released");
            TakeImage();
        }
    }

    void CheckTrigger()
    {
        if (_controller.TriggerValue > 0.2f)
        {
            if (pressed == false)
            {
                Debug.Log("pressed Trigger");
                TakeImage();
            }
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }
    #endregion input
    #region camera
    /// <summary>
    /// Once privileges have been granted, enable the camera and callbacks.
    /// </summary>
    private void StartCapture()
    {
        if (!_hasStarted)
        {
            lock (_cameraLockObject)
            {
                EnableMLCamera();

#if PLATFORM_LUMIN
                MLCamera.OnRawImageAvailable += OnCaptureRawImageComplete;
#endif
            }

            _granted = true;
        }
    }

    /// <summary>
    /// Connects the MLCamera component and instantiates a new instance
    /// if it was never created.
    /// </summary>
    private void EnableMLCamera()
    {
        lock (_cameraLockObject)
        {
            MLResult result = MLCamera.Start();
            Debug.Log("camerastart");
            if (result.IsOk)
            {
                result = MLCamera.Connect();
                _isCameraConnected = true;
            }
            else
            {
                Debug.LogErrorFormat("Error: ImageCaptureExample failed starting MLCamera, disabling script. Reason: {0}", result);
                enabled = false;
                return;
            }
        }
    }

    /// <summary>
    /// Disconnects the MLCamera if it was ever created or connected.
    /// </summary>
    private void DisableMLCamera()
    {
#if PLATFORM_LUMIN
        lock (_cameraLockObject)
        {
            if (MLCamera.IsStarted)
            {
                MLCamera.Disconnect();
                // Explicitly set to false here as the disconnect was attempted.
                _isCameraConnected = false;
                MLCamera.Stop();
            }
        }
#endif
    }
    #endregion camera

    void TakeImage() //called by button presses
    {
        if (_granted)
        {
            Debug.Log("take image do");
            TriggerAsyncCapture();

        }
    }
    /// <summary>
    /// Captures a still image using the device's camera and returns
    /// the data path where it is saved.
    /// </summary>
    /// <param name="fileName">The name of the file to be saved to.</param>
    public void TriggerAsyncCapture()
    {
        if (_captureThread == null || (!_captureThread.IsAlive))
        {
            Debug.Log("starting image take thread");
            ThreadStart captureThreadStart = new ThreadStart(CaptureThreadWorker);
            _captureThread = new Thread(captureThreadStart);
            _captureThread.Start();
        }
        else
        {
            Debug.Log("Previous thread has not finished, unable to begin a new capture just yet.");
        }
    }

    /// <summary>
    /// Worker function to call the API's Capture function
    /// </summary>
    private void CaptureThreadWorker()
    {
        Debug.Log(MLCamera.IsStarted + " " + _isCameraConnected);
        lock (_cameraLockObject)
        {
            if (MLCamera.IsStarted && _isCameraConnected)
            {
                MLResult result = MLCamera.CaptureRawImageAsync();
                if (result.IsOk)
                {
                    _isCapturing = true;
                }
            }
        }
    }

    /// <summary>
    /// Handles the event of a new image getting captured.
    /// </summary>
    /// <param name="imageData">The raw data of the image.</param>
    private void OnCaptureRawImageComplete(byte[] imageData)
    {
        lock (_cameraLockObject)
        {
            _isCapturing = false;
        }
        // Initialize to 8x8 texture so there is no discrepency
        // between uninitalized captures and error texture
        StartCoroutine(VisionManager.instance.AnalyseImage(imageData));
        Texture2D texture = new Texture2D(8, 8);
        bool status = texture.LoadImage(imageData);

        if (status && (texture.width != 8 && texture.height != 8))
        {
            _previewObject.SetActive(true);
            Renderer renderer = _previewObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
            }
        }
    }
}

