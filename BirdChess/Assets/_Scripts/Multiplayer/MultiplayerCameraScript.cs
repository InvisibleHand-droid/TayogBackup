using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class MultiplayerCameraScript : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    private void Start() {
        if(!PhotonNetwork.IsMasterClient)
        {
            FlipCamera();
        }
    }

    private void FlipCamera()
    {
        cinemachineCamera.m_XAxis.Value = 180;
    }
}
