using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BouncePadsController : MonoBehaviour
{
    private InputManager _inputManager;
    private Camera _cameraMain;

    [SerializeField] private Transform leftBouncePad, rightBouncePad;
    [SerializeField, Range(0f, 20f)] private float keyboardPadSpeed = 2f;

    private int leftTouchId = -1, rightTouchId = -1;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _cameraMain = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        leftBouncePad = GameObject.Find("Left Bounce Pad").transform;
        rightBouncePad = GameObject.Find("Right Bounce Pad").transform;
    }

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            _inputManager.OnStayLeftAxis += KeyBoardMove;
            _inputManager.OnStayRightAxis += KeyBoardMove;
        }
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            _inputManager.OnStayLeftAxis -= KeyBoardMove;
            _inputManager.OnStayRightAxis -= KeyBoardMove;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void TouchMove()
    {
        if (Touch.activeTouches.Count > 0)
        {
            foreach (Touch touch in Touch.activeTouches)
            {
                if (touch.touchId == leftTouchId && touch.ended)
                {
                    leftTouchId = -1;
                    continue;
                }

                if (touch.touchId == rightTouchId && touch.ended)
                {
                    rightTouchId = -1;
                    continue;
                }

                Vector2 touch2D = touch.screenPosition;
                // Vector3 touch3D = new Vector3(touch2D.x, touch2D.y, -_cameraMain.transform.position.z);
                Vector3 world3D = _cameraMain.ScreenToWorldPoint(touch2D);

                if (world3D.x <= 0)
                {
                    if (rightTouchId == touch.touchId) rightTouchId = -1;
                    if (leftTouchId == -1) leftTouchId = touch.touchId;

                    if (leftTouchId != touch.touchId) continue;
                    world3D.x = -10;
                    world3D.z = 0;
                    leftBouncePad.transform.position = world3D;
                }
                else
                {
                    if (leftTouchId == touch.touchId) leftTouchId = -1;
                    if (rightTouchId == -1) rightTouchId = touch.touchId;
                    if (rightTouchId != touch.touchId) continue;
                    world3D.x = 10;
                    world3D.z = 0;
                    rightBouncePad.transform.position = world3D;
                }
            }
        }
    }

    private void KeyBoardMove(float axis, int id, float time)
    {
        // Debug.Log(_inputControls.Keyboard.LeftAxis.ReadValue<float>());
        switch (id)
        {
            case 0:
            {
                var pos = leftBouncePad.position;
                pos.y += axis * Time.deltaTime * keyboardPadSpeed;
                leftBouncePad.position = pos;
                break;
            }
            case 1:
            {
                var pos = rightBouncePad.position;
                pos.y += axis * Time.deltaTime * keyboardPadSpeed;
                rightBouncePad.position = pos;
                break;
            }
        }
    }
}