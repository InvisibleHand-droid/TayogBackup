using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : SingletonPersistent<NetworkManager>, ILobbyCallbacks
{
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

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.LoadLevel(multiplayerSceneBuildIndex);
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to server. Looking for random room");
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
        base.OnLeftRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError(newPlayer + "entered the room");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Debug.Log("start");
            StartGame();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogError(otherPlayer + "left the room");
        StartCoroutine(AutoDisconnect());
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

    IEnumerator AutoDisconnect()
    {
        yield return new WaitForSeconds(5f);

        PhotonNetwork.LeaveRoom();
        Debug.LogError("A player has disconnected so room was closed.");

        yield return null;
    }
}
