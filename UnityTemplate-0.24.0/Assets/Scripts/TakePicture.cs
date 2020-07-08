using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using System.Threading;


public class TakePicture : MonoBehaviour
{
    // Start is called before the first frame update
    private MLInput.Controller _controller;
    private MLPrivilegeRequesterBehavior _privRequester = null;
    private bool _granted = false;
    private object _cameraLockObject = new object();

    private bool _isCameraConnected = false;

    private bool _isCapturing = false;

    private bool _hasStarted = false;

    private bool _privilegesBeingRequested = false;

    private Thread _captureThread = null;

    void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
        MLInput.OnControllerButtonUp += OnButtonUp;
    }

    private void OnDestroy()
    {
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
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

    /// <summary>
    /// Connects the MLCamera component and instantiates a new instance
    /// if it was never created.
    /// </summary>
    private void EnableMLCamera()
    {
        lock (_cameraLockObject)
        {
            MLResult result = MLCamera.Start();
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

    void HandlePrivilegesDone(MLResult result)
    {
        if(!result.IsOk)
        {
            Debug.Log("Error: Priv Request Failed");
        }
        else
        {
            Debug.Log("Success.Priv granted");
            _granted = true;

            lock (_cameraLockObject)
            {
                EnableMLCamera();
                MLCamera.OnRawImageAvailable += OnCaptureRawImageComplete;
            }
        }
    }
    private void Awake()
    {
        _privRequester = GetComponent<MLPrivilegeRequesterBehavior>();
        _privRequester.OnPrivilegesDone += HandlePrivilegesDone;
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
        if(_controller.TriggerValue > 0.2f)
        {
            if(pressed == false)
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

    void TakeImage()
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

        Debug.Log("Image Done");
        // Initialize to 8x8 texture so there is no discrepency
        // between uninitalized captures and error texture
        //has image now
    }
}

