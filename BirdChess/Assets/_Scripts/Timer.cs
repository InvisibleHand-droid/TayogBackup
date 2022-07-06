using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Timer : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int _minutes;
    [SerializeField] protected int _seconds;
    protected float _timer;
    [SerializeField] protected bool isIncrement;
    public bool isTimerOn;

    public virtual void Awake()
    {
        _timer = (_minutes * 60) + _seconds;
    }

    public virtual void Start()
    {
        isTimerOn = false;
    }

    public virtual void Update()
    {
        _timer += isIncrement ? Time.deltaTime : -Time.deltaTime;
    }

    public string GetTimerString()
    {
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TurnTimerOn()
    {
        isTimerOn = true;
    }

    public void TurnTimerOff()
    {
        isTimerOn = false;
    }

    public virtual void ResetTimer()
    {
        _timer = isIncrement ? 0 : (_minutes * 60) + _seconds;
    }
}
