using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeCam : MonoBehaviour
{
    [SerializeField] float intensity = 5f;
    CinemachineVirtualCamera myVirtualCam;
    // Start is called before the first frame update
    void Start()
    {
        myVirtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin noise = myVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = intensity;
    }
}
