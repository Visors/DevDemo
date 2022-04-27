using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Vector3 nowPosition, targetPosition;
    [SerializeField] private Quaternion nowRotation, targetRotation;
    private int _transCode = 0;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        nowPosition = _transform.position;
        nowRotation = _transform.rotation;
        targetRotation = Quaternion.Euler(Vector3.zero);
    }

    public void Click()
    {
        // _transCode = 1;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        while (nowPosition != targetPosition && nowRotation != targetRotation)
        {
            yield return new WaitForFixedUpdate(); //帧执行
            nowPosition = Vector3.Lerp(nowPosition, targetPosition, Time.fixedDeltaTime);
            nowRotation = Quaternion.Lerp(nowRotation, targetRotation, Time.fixedDeltaTime);
            _transform.position = nowPosition;
            _transform.rotation = nowRotation;
        }

        _transform.position = nowPosition = targetPosition;
        _transform.rotation = nowRotation = targetRotation;
        yield return new WaitForSeconds(1);
        GetComponent<Camera>().orthographic = true;
    }

    private void FixedUpdate()
    {
        if (_transCode == 1)
        {
            if (nowPosition != targetPosition && nowRotation != targetRotation)
            {
                nowPosition = Vector3.Lerp(nowPosition, targetPosition, Time.deltaTime);
                nowRotation = Quaternion.Lerp(nowRotation, targetRotation, Time.deltaTime);
                _transform.position = nowPosition;
                _transform.rotation = nowRotation;
            }
            else _transCode = 0;
        }

        // Debug.Log(_transCode);
    }
}