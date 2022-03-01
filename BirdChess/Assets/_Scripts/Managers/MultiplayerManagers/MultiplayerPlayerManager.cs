using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerPlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerUI;

    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Instance.isPhotonOffline)
        {
            for (int i = 1; i < 3; i++)
            {
                MakePlayer(i);
            }
        }
        else
        {
            MakePlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }

    }

    private void MakePlayer(int i)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, this.transform.position, Quaternion.identity);
        player.tag = "Player" + i;
    }
}
