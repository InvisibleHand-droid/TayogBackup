using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{
    [Range(0f, 10f)] public float LookSpeed = 1f;
    public bool InvertY = false;
    [SerializeField] private CinemachineFreeLook cinemachineVirtualCamera;
    private Vector2 defaultPos;
    private bool isCoroutineDone;

    //temporary, will most likely change when implementing multiplayer camera controls
    private string activePlayer = "P1";
    private float P1_XValue = 0;
    private float P1_XRange = 180;
    private float P2_XValue = 180;
    private float P2_XRange = 0;
    //end of note

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineFreeLook>();
        isCoroutineDone = true;
        defaultPos = new Vector2(cinemachineVirtualCamera.m_XAxis.Value, cinemachineVirtualCamera.m_YAxis.Value);
    }
    public void OnMoveCamera(InputAction.CallbackContext _value)
    {
        //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
        Vector2 lookMovement = _value.ReadValue<Vector2>().normalized;
        lookMovement.y = InvertY ? -lookMovement.y : lookMovement.y;

        // This is because X axis is only contains between -180 and 180 instead of 0 and 1 like the Y axis
        lookMovement.x = lookMovement.x * 180f;

        //Ajust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
        cinemachineVirtualCamera.m_XAxis.Value += lookMovement.x * LookSpeed * Time.deltaTime;
        cinemachineVirtualCamera.m_YAxis.Value += lookMovement.y * LookSpeed * Time.deltaTime;

    }
    public void OnResetCamera(InputAction.CallbackContext _value)
    {
        if (isCoroutineDone)
        {
            isCoroutineDone = false;
            StartCoroutine(ReturnCamera(defaultPos, 1));
        }
    }
    private IEnumerator ReturnCamera(Vector2 endValue, float duration)
    {

        float time = 0;
        Vector2 axisValues = new Vector2(cinemachineVirtualCamera.m_XAxis.Value, cinemachineVirtualCamera.m_YAxis.Value);
        Vector2 startValue = axisValues;
        float t = time / duration;

        while (time < duration)
        {
            t = time / duration;
            t = t * t * (3f - 2f * t);
            axisValues = Vector2.Lerp(startValue, endValue, t);
            cinemachineVirtualCamera.m_XAxis.Value = axisValues.x;
            cinemachineVirtualCamera.m_YAxis.Value = axisValues.y;
            time += Time.deltaTime;
            yield return null;
        }
        cinemachineVirtualCamera.m_XAxis.Value = endValue.x;
        cinemachineVirtualCamera.m_YAxis.Value = endValue.y;
        isCoroutineDone = true;
    }

    public void SwitchPlayer()
    {
        switch (activePlayer)
        {
            case "P1":
                activePlayer = "P2";
                Debug.Log("active player: " + activePlayer);
                //cinemachineVirtualCamera.m_XAxis.m_MinValue = -P2_XRange;
                //cinemachineVirtualCamera.m_XAxis.m_MaxValue = P2_XRange;
                defaultPos = new Vector2(P2_XValue, 0.5f);
                StartCoroutine(ReturnCamera(defaultPos, 1));
                //switch from p1 to p2 always turn to the left
                break;
            case "P2":
                activePlayer = "P1";
                Debug.Log("active player: " + activePlayer);
                //cinemachineVirtualCamera.m_XAxis.m_MinValue = -P1_XRange;
                //cinemachineVirtualCamera.m_XAxis.m_MaxValue = P1_XRange;
                defaultPos = new Vector2(P1_XValue, 0.5f);
                StartCoroutine(ReturnCamera(defaultPos, 1));
                //switch from p2 to p1 always turn to the right
                break;
        }
    }
}
