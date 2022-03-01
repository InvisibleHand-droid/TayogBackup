using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkSinglePlayer : MonoBehaviour
{
    public void StartSinglePlayer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.OfflineMode = true;
        NetworkManager.Instance.isPhotonOffline = true;
        PhotonNetwork.LoadLevel(NetworkManager.Instance.multiplayerSceneBuildIndex);
    }
}
