using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI connectionStatus;
    public GameObject multiplayerPanelUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        connectionStatus.SetText(PhotonNetwork.NetworkClientState.ToString());
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    
    public void Connect()
    {
        if (PhotonNetwork.OfflineMode) return;
        Debug.Log("tried to connect");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

}
