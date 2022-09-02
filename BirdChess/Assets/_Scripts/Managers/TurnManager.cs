using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private GameEventChannel _onTurnEnd;
    private int _playerNumber;

    public override void Awake()
    {
        base.Awake();
    }

    [PunRPC]
    public void RPCSetCurrentPlayer(int color)
    {
        TeamColor teamColor = TeamColor.White;

        switch (color)
        {
            case 1:
                teamColor = TeamColor.White;
                break;
            case 2:
                teamColor = TeamColor.Black;
                break;
            default:
                break;
        }

        foreach (Player player in GameManager.Instance.players)
        {
            if (player.teamColor.Equals(teamColor))
            {
                _playerNumber = GameManager.Instance.players.IndexOf(player);
                player.playerMove.isMyTurn = true;
            }
            else
            {
                player.playerMove.isMyTurn = false;
            }
        }
    }


    [PunRPC]
    public void RPCNextTurn()
    {
        _playerNumber = GetNextPlayer();
        Player playerTarget = GameManager.Instance.players[_playerNumber];
        Player playerFromList = null;
        //Look through player list, make the selected player's turn bool true
        foreach (Player player in GameManager.Instance.players)
        {
            if (player == playerTarget)
            {
                playerFromList = player;
            }
            player.playerMove.isMyTurn = false;
//            player.timer.isTimerOn = false;
        }

        playerFromList.playerMove.isMyTurn = true;
//        playerFromList.timer.isTimerOn = false;
        _onTurnEnd.Raise();
    }

    private int GetNextPlayer()
    {
        int nextPlayer = _playerNumber;

        if (nextPlayer + 1 >= GameManager.Instance.players.Count)
        {
            nextPlayer = 0;
        }
        else
        {
            nextPlayer++;
        }

        return nextPlayer;
    }

    public Player GetCurrentPlayer()
    {
        return GameManager.Instance.players[_playerNumber];
    }


    public Player GetPlayerReferenceBasedOnColor(TeamColor teamColor)
    {
        foreach (Player player in GameManager.Instance.players)
        {
            if (player.teamColor.Equals(teamColor))
            {
                return player;
            }
        }
        return null;
    }

}
