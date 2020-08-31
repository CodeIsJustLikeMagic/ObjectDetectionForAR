using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class InputHandler : MonoBehaviour
{
    //Trigger for TakeImage
    //Home button to reorient canvas
    //Bumper t
    private MLInput.Controller _controller;

    private void TriggerTapped()
    {
        Debug.Log("Trigger Tapped");
        TakePicture.instance.TakeImage();
    }
    private void HomeButtonTapped()
    {
        Debug.Log("Home Button Tapped");
        MoveDisplay.instance.ReorientCanvas();
    }
    private void BumperTapped()
    {
        Debug.Log("Bumper Tapped");
        
        LabelCreater.instance.ToggleShow();
    }
    private void BumperHeld()
    {
        Debug.Log("Bumper Held");
        LabelCreater.instance.EraseLast();
    }
    
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
            // Add button callbacks
            MLInput.OnControllerButtonDown += OnButtonDown;
            MLInput.OnControllerButtonUp += OnButtonUp;


            // Initial State of the Control is Normal
            BtnState = ButtonStates.Normal;
        }
        catch (System.Exception e)
        {
            Debug.Log("couldnt find ML Controller");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Bumper button held down - toggle scanning if timer reaches max
        if (GetTime() >= TIME_MESH_SCANNING_TOGGLE && BtnState == ButtonStates.Pressed)
        {
            //bumper was held.
            BumperHeld();
            _held = true;
            _startTime = Time.time;
            //_meshing.ToggleMeshScanning();
        }
        // Bumper was just released - toggle visibility
        else if (BtnState == ButtonStates.JustReleased)
        {
            BtnState = ButtonStates.Normal;
            _startTime = 0.0f;
            if (!_held)
            {
                BumperTapped();
                //bumper was tapped.
                //_meshing.ToggleMeshVisibility();
            }
            else
            {
                _held = false;
            }
        }
        CheckTrigger();
    }

    #region Public Variables 
    public enum ButtonStates
    {
        Normal,
        Pressed,
        JustReleased
    };
    public ButtonStates BtnState;
    #endregion

    #region Private Variables
    private const float TIME_MESH_SCANNING_TOGGLE = 1.5f;
    private bool _held = false;
    private float _startTime = 0.0f;
    #endregion

    private void OnDestroy()
    {
        // Stop input
        MLInput.Stop();

        // Remove button callbacks
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
    }
    
    public float GetTime()
    {
        float returnTime = -1.0f;
        if (_startTime > 0.0f)
        {
            returnTime = Time.time - _startTime;
        }
        return returnTime;
    }


    void OnButtonDown(byte controller_id, MLInput.Controller.Button button)
    {
        // Callback - Button Down
        if (button == MLInput.Controller.Button.Bumper)
        {
            // Start bumper timer
            _startTime = Time.time;
            BtnState = ButtonStates.Pressed;
        }
    }

    //on bumper press
    void OnButtonUp(byte controllerId, MLInput.Controller.Button button)
    {
        // Callback - Button Up
        if (button == MLInput.Controller.Button.Bumper)
        {
            BtnState = ButtonStates.JustReleased;
            //Labelcreater toogle.
        }
        if (button == MLInput.Controller.Button.HomeTap)
        {
            HomeButtonTapped();
        }
    }
    
    bool pressed = false;
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
                TriggerTapped();
            }
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }
}
