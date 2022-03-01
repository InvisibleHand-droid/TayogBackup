using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private GameEventChannel onTurnEnd;
    private int _playerNumber;

    public override void Awake()
    {
        base.Awake();
    }

    [PunRPC]
    public void RPCSetPlayerColors()
    {
        Debug.Log("called");
        //Decide the player
        _playerNumber = Random.Range(0, GameManager.Instance.players.Count);
        Player playerTarget = GameManager.Instance.players[_playerNumber];
        Player playerFromList = null;

        //Look through player list, make the selected player's turn bool true
        foreach (Player player in GameManager.Instance.players)
        {
            if (player == playerTarget)
            {
                playerFromList = playerTarget;
            }
            player._teamColor = TeamColor.White;
            player.name = player._teamColor.ToString() + " Player";
        }
        playerFromList._teamColor = TeamColor.Black;
        playerFromList.name = playerFromList._teamColor.ToString() + " Player";
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
            if (player._teamColor.Equals(teamColor))
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


    public void NextTurn()
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
        }

        playerFromList.playerMove.isMyTurn = true;

        onTurnEnd.Raise();
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
            if (player._teamColor.Equals(teamColor))
            {
                return player;
            }
        }
        return null;
    }

}
