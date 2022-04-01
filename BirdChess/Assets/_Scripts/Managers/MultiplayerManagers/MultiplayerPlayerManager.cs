using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerPlayerManager : MonoBehaviourPun
{
    public GameObject playerPrefab;
    public GameObject playerUI;
    public static int playerCount = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        playerCount = 0;
    }
    void Start()
    {

        if (NetworkManager.Instance.isPhotonOffline)
        {
            for (int i = 0; i < 2; i++)
            {
                MakePlayer();
            }
        }
        else
        {
            MakePlayer();
        }

    }

    private void MakePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, this.transform.position, Quaternion.identity);
    }
}
