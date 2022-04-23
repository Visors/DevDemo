using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class ApplicationSettings : MonoBehaviourSingleton<ApplicationSettings>
{
    private void Awake()
    {
        // Lock 60 fps
        Application.targetFrameRate = 60;
        // Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}