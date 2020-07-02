using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class ImageCapture : MonoBehaviour
{
    public static ImageCapture instance;
    public int tapsCount;
    private UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject = null;
    private GestureRecognizer recognizer;
    private bool currentlyCapturing = false;
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        instance = this;
    }

    void Start()
    {
        // subscribing to the HoloLens API gesture recognizer to track user gestures
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += TapHandler;
        recognizer.StartCapturingGestures();
    }

    /// <summary>
    /// Respond to Tap Input.
    /// </summary>
    private void TapHandler(TappedEventArgs obj)
    {
        // Only allow capturing, if not currently processing a request.
        if (currentlyCapturing == false)
        {
            currentlyCapturing = true;

            // increment taps count, used to name images when saving
            tapsCount++;

            // Create a label in world space using the ResultsLabel class
            ResultsLabel.instance.CreateLabel();

            // Begins the image capture and analysis procedure
            ExecuteImageCaptureAndAnalysis();
        }
    }
    /// <summary>
    /// Register the full execution of the Photo Capture. If successful, it will begin 
    /// the Image Analysis process.
    /// </summary>
    void OnCapturedPhotoToDisk(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // Call StopPhotoMode once the image has successfully captured
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // Dispose from the object in memory and request the image analysis 
        // to the VisionManager class
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        StartCoroutine(VisionManager.instance.AnalyseLastImageCaptured());
    }

    /// <summary>    
    /// Begin process of Image Capturing and send To Azure     
    /// Computer Vision service.   
    /// </summary>    
    private void ExecuteImageCaptureAndAnalysis()
    {
        // Set the camera resolution to be the highest possible    
        Resolution cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Begin capture process, set the image format    
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            UnityEngine.Windows.WebCam.CameraParameters camParameters = new UnityEngine.Windows.WebCam.CameraParameters();
            camParameters.hologramOpacity = 0.0f;
            camParameters.cameraResolutionWidth = targetTexture.width;
            camParameters.cameraResolutionHeight = targetTexture.height;
            camParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

            // Capture the image from the camera and save it in the App internal folder    
            captureObject.StartPhotoModeAsync(camParameters, delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
            {
                string filename = string.Format(@"CapturedImage{0}.jpg", tapsCount);

                string filePath = Path.Combine(Application.persistentDataPath, filename);

                VisionManager.instance.imagePath = filePath;

                photoCaptureObject.TakePhotoAsync(filePath, UnityEngine.Windows.WebCam.PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);

                currentlyCapturing = false;
            });
        });
    }
}
