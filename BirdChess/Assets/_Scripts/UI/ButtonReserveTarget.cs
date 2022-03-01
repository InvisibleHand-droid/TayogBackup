using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonReserveTarget : MonoBehaviour
{
    public Player player;
    public PieceType pieceType;

    private void Start()
    {
        StartCoroutine(CoroutineInitializeTargetPlayer());
    }

    public void InitializeTargetPlayer()
    {
        if (NetworkManager.Instance.isPhotonOffline)
        {
            UIManager.Instance.SetUIDependencies();
        }
        else
        {
            foreach (Player player in GameManager.Instance.players)
            {
                if (player.photonView.IsMine)
                {
                    this.player = player;
                }
            }
        }
    }

    IEnumerator CoroutineInitializeTargetPlayer()
    {
        while (GameManager.Instance.players.Count <= 0)
        {
            yield return null;
        }

        InitializeTargetPlayer();
    }
}
