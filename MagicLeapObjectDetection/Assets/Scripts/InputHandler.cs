using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class InputHandler : MonoBehaviour
{
    private MLInput.Controller _controller;
    void Start()
    {
        try
        {
            //get user input. we are using bumper and trigger
            MLInput.Start();
            _controller = MLInput.GetController(MLInput.Hand.Left);
            if (_controller == null)
            {
                Debug.Log("Controller not found. Did you start Lab?");
                enabled = false;
            }
            MLInput.OnControllerButtonUp += OnButtonUp;
        }
        catch (System.Exception e)
        {
            Debug.Log("coudnlt find Controller");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CheckTrigger();
    }

    bool pressed = false;

    //on bumper press
    void OnButtonUp(byte controllerId, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.Bumper)
        {
            Debug.Log("bumper released");
            TestLabels.instance.ShowPos();
        }
        if(button == MLInput.Controller.Button.HomeTap)
        {
            PixelToWorld.instance.markEdges(Camera.main.cameraToWorldMatrix);
        }
    }

    void CheckTrigger()
    {
        if (_controller == null)
        {
            return;
        }
        if (_controller.TriggerValue > 0.2f)
        {
            if (pressed == false)
            {
                Debug.Log("pressed Trigger");
                TakePicture.instance.TakeImage();
            }
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }
    public void OnDestroy()
    {
        //clean up input
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
    }
}
