using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : SingletonPersistent<NetworkManager>, ILobbyCallbacks
{
    [SerializeField] TextMeshProUGUI connectionStatus;
    public GameObject multiplayerPanelUI;
    public bool isPhotonOffline;
    public int multiplayerSceneBuildIndex;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (isPhotonOffline)
        {
            PhotonNetwork.OfflineMode = true;
        }
    }

    private void Update()
    {
        connectionStatus.SetText(PhotonNetwork.NetworkClientState.ToString());
    }

    public void Connect()
    {
        if (isPhotonOffline) return;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.LoadLevel(multiplayerSceneBuildIndex);
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to server. Looking for random room");

        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            multiplayerPanelUI.SetActive(true);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join Failed because {message}");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
    }

    public override void OnLeftRoom()
    {
        Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} left the room");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError(($"Player disconnected because of {cause}"));
    }

    public override void OnLeftLobby()
    {
        Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} left the lobby");
    }

    public override void OnCreatedRoom()
    {
        Debug.LogError("Created room success");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed because {message}");
    }
    #endregion
}
