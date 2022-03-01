using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class NetworkPrivateMatch : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roomIDInput;
    [SerializeField] private TextMeshProUGUI _roomIDText;

    private const string characters = "abcdefghijklmnopqrstuvwxyz09123456789";

    // Update is called once per frame
    public void CreatePrivateRoom()
    {
        string RandomRoomID = GetRandomRoomID();
        _roomIDText.SetText(RandomRoomID);
        PhotonNetwork.CreateRoom(RandomRoomID);
    }

    public void JoinPrivateRoom()
    {
        PhotonNetwork.JoinRoom(_roomIDInput.text);
    }

    public void ExitPrivateRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private string GetRandomRoomID()
    {
        //Check if RoomID is currently not being in use then make that a room id else generate again
        string RandomRoomID = GenerateRandomRoomID();

        return RandomRoomID;
    }

    private string GenerateRandomRoomID()
    {
        string randomString = "";
        for (int i = 0; i < 6; i++)
        {
            randomString += characters[Random.Range(0, characters.Length - 1)];
        }
        return (randomString);
    }
}
