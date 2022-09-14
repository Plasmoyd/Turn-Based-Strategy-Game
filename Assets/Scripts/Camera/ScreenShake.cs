using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance;

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("More than once instance of object");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float force = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(force);
    }
}
