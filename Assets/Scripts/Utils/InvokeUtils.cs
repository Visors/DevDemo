using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class InvokeUtils : MonoBehaviour
    {
        public static IEnumerator DelayInvoke(Action action, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            action();
        }
    }
}