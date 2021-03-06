using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    private InputManager _inputManager;
    private Camera _cameraMain;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        _inputManager.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        _inputManager.OnStartTouch -= Move;
    }

    private void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, _cameraMain.nearClipPlane);
        Vector3 worldCoordinates = _cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }
}