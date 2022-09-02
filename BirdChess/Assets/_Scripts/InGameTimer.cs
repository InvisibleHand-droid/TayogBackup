using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGameTimer : Timer
{
    [SerializeField] private int targetUI;
    public override void Awake()
    {

        _minutes = GameManager.Instance.gameSettingsData.minutes;
        _seconds = GameManager.Instance.gameSettingsData.seconds;

        base.Awake();
    }

    public override void Update()
    {
        if (isTimerOn)
        {
            //base.Update();
            //UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCUpdateTimerText), RpcTarget.All, targetUI, GetTimerString());
            //UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCUpdateTimerText), RpcTarget.MasterClient, targetUI, GetTimerString());
        }

       // if (this.photonView.IsMine)
       // {
        //    Debug.Log(_timer);
        //}
    }
}
