using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkQuickMatch : MonoBehaviour
{
    public void QuickMatch()
    {
        if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ExitQuickMatch()
    {
        PhotonNetwork.LeaveRoom();
    }
}
