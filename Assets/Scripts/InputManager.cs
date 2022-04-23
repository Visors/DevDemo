using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);

    public event StartTouchEvent OnStartTouch;

    public delegate void PerformTouchEvent(Vector2 position, float time);

    public event PerformTouchEvent OnPerformTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);

    public event EndTouchEvent OnEndTouch;

    public delegate void StartLeftAxisEvent(float axis, float time);

    public event StartLeftAxisEvent OnStartLeftAxis;

    public delegate void StartRightAxisEvent(float axis, float time);

    public event StartRightAxisEvent OnStartRightAxis;

    public delegate void EndLeftAxisEvent(float axis, float time);

    public event EndLeftAxisEvent OnEndLeftAxis;

    public delegate void EndRightAxisEvent(float axis, float time);

    public event EndRightAxisEvent OnEndRightAxis;

    public delegate void StayLeftAxisEvent(float axis, int id, float time);

    public event StayLeftAxisEvent OnStayLeftAxis;

    public delegate void StayRightAxisEvent(float axis, int id, float time);

    public event StayRightAxisEvent OnStayRightAxis;

    private InputControls _inputControls;
    private bool _onStayLeftAxis = false, _onStayRightAxis = false;


    private void Awake()
    {
        _inputControls = new InputControls();
    }

    private void OnEnable()
    {
        _inputControls.Enable();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        _inputControls.Disable();
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        // _inputControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        // _inputControls.Touch.TouchPress.performed += ctx => PerformTouch(ctx);
        // _inputControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
        _inputControls.Keyboard.LeftAxis.started += StartLeftAxis;
        _inputControls.Keyboard.RightAxis.started += StartRightAxis;
        _inputControls.Keyboard.LeftAxis.canceled += EndLeftAxis;
        _inputControls.Keyboard.RightAxis.canceled += EndRightAxis;
        _inputControls.Keyboard.LeftAxis.started += StartLeftAxis;
        _inputControls.Keyboard.RightAxis.started += StartRightAxis;
        _inputControls.Keyboard.LeftAxis.canceled += EndLeftAxis;
        _inputControls.Keyboard.RightAxis.canceled += EndRightAxis;
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        // Debug.Log("Touch started " + _inputControls.Touch.TouchPosition.ReadValue<Vector2>());
        OnStartTouch?.Invoke(_inputControls.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.startTime);
    }

    private void PerformTouch(InputAction.CallbackContext context)
    {
        // Debug.Log("Touch performed " + _inputControls.Touch.TouchPosition.ReadValue<Vector2>());
        OnPerformTouch?.Invoke(_inputControls.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.time);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        // Debug.Log("Touch ended");
        OnEndTouch?.Invoke(_inputControls.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.time);
    }

    private void FingerDown(Finger finger)
    {
        // Debug.Log(Touch.activeTouches);
        OnStartTouch?.Invoke(finger.screenPosition, Time.time);
    }

    private void StartLeftAxis(InputAction.CallbackContext context)
    {
        _onStayLeftAxis = true;
        // Debug.Log("Left Axis started " + context);
        OnStartLeftAxis?.Invoke(_inputControls.Keyboard.LeftAxis.ReadValue<float>(), (float) context.startTime);
    }

    private void StartRightAxis(InputAction.CallbackContext context)
    {
        _onStayRightAxis = true;
        // Debug.Log("Right Axis started " + context);
        OnStartRightAxis?.Invoke(_inputControls.Keyboard.RightAxis.ReadValue<float>(), (float) context.startTime);
    }

    private void EndLeftAxis(InputAction.CallbackContext context)
    {
        _onStayLeftAxis = false;
        // Debug.Log("Left Axis ended " + context);
        OnEndLeftAxis?.Invoke(_inputControls.Keyboard.LeftAxis.ReadValue<float>(), (float) context.time);
    }

    private void EndRightAxis(InputAction.CallbackContext context)
    {
        _onStayRightAxis = false;
        // Debug.Log("Right Axis ended " + context);
        OnEndRightAxis?.Invoke(_inputControls.Keyboard.RightAxis.ReadValue<float>(), (float) context.time);
    }

    private void Update()
    {
        if (_onStayLeftAxis)
        {
            // Debug.Log("Left Axis stayed " + Time.time);
            OnStayLeftAxis?.Invoke(_inputControls.Keyboard.LeftAxis.ReadValue<float>(), 0, Time.time);
        }

        if (_onStayRightAxis)
        {
            // Debug.Log("Right Axis stayed " + Time.time);
            OnStayRightAxis?.Invoke(_inputControls.Keyboard.RightAxis.ReadValue<float>(), 1, Time.time);
        }
    }
}