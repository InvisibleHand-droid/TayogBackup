using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class MultiplayerCameraScript : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cinemachineCamera;

    private void Awake()
    {
        _cinemachineCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    private void Start() {
        if(!PhotonNetwork.IsMasterClient)
        {
            FlipCamera();
        }
    }

    private void FlipCamera()
    {
        _cinemachineCamera.m_XAxis.Value = 0;
    }
}
