using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkQuickMatch : MonoBehaviour
{
    public void QuickMatch()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(null,2, MatchmakingMode.FillRoom, TypedLobby.Default, null, null, options);
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
