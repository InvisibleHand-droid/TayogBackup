using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum WinType
{
    Capture,
    Dominion,
    StaleMate
}

[System.Serializable]
public class GameSettings
{
    public int ManokCount;
    public int BibeCount;
    public int LawinCount;
    public int AgilaCount;
}

public class GameManager : Singleton<GameManager>
{
    public GameSettings gameSettings;

    public TayogSetCollection tayogSetCollection;

    public List<Player> players = new List<Player>();
    WaitForSeconds shortWait = new WaitForSeconds(1f);
    //win conditions
    //at the start of player's turn, if they can't make any action/move, opponent win
    //when a piece is added to reserve, count Manok, if 8, player win
    //at the end of a player's action/move, if no Manok of the opponents send their headcount, player win
    public static GameState _currentGameState;
    public override void Awake()
    {
        base.Awake();

#if UNITY_STANDALONE
        Screen.SetResolution(564, 960, false);
        Screen.fullScreen = false;
#endif
    }

    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            StartCoroutine(LocalSetupSequence());
        }
        else
        {
            StartCoroutine(OnlineSetupSequence());
        }
    }

    [PunRPC]
    public void SetCurrentGameState(GameState gameState)
    {
        _currentGameState = gameState;
    }

    public void CheckPlayerVictoryConditions()
    {
        if (GameManager._currentGameState != GameState.GoingOn) return;

        foreach (Player player in players)
        {
            if (player.GetReserveCountOfPieceType(PieceType.Manok) >= 8)
            {
                UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, player._teamColor, "win");
                _currentGameState = GameState.End;
                break;
            }

            if (player.GetManokActiveCount() <= 0)
            {
                UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, player._teamColor, "lose");
                _currentGameState = GameState.End;
                break;
            }

            if (NoValidMoves())
            {
                UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, player._teamColor, "has no valid moves");
                _currentGameState = GameState.End;
                break;
            }
        }
    }

    public bool NoValidMoves()
    {
        Player player = TurnManager.Instance.GetCurrentPlayer();
        List<TayogPiece> allTayogPiece = new List<TayogPiece>();

        bool hasNoMoves = false;
        foreach (TayogPiece reserveTayogPiece in player._reserveTayogPiece)
        {
            allTayogPiece.Add(reserveTayogPiece);
        }

        foreach (TayogPiece activeTayogPiece in player._activeTayogPiece)
        {
            allTayogPiece.Add(activeTayogPiece);
        }

        foreach (TayogPiece tayogPiece in allTayogPiece)
        {
            if (player.previouslyPlayedTayogPiece != tayogPiece)
            {
                if (tayogPiece.GetValidTiles() == null)
                {
                    hasNoMoves = true;
                    break;
                }
            }
        }

        return hasNoMoves;
    }

    IEnumerator OnlineSetupSequence()
    {
        Player localPlayer = null;
        Player nonLocalPlayer = null;
        while (players.Count != 2)
        {
            yield return null;
        }

        /* foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
         {
             if (player.IsLocal)
             {
                 localPlayer = player.TagObject;
             }
         }*/

        foreach (Player player in players)
        {
            if (player.photonView.IsMine)
            {
                localPlayer = player;
            }
            else
            {
                nonLocalPlayer = player;
            }
        }

        UIManager.Instance.SetUIDependencies(0, localPlayer);
        UIManager.Instance.SetPlayerHeaderTexts(0, localPlayer);

        UIManager.Instance.SetUIDependencies(1, nonLocalPlayer);
        UIManager.Instance.SetPlayerHeaderTexts(1, nonLocalPlayer);

        if (!PhotonNetwork.IsMasterClient) yield break;

        this.photonView.RPC(nameof(SetCurrentGameState), RpcTarget.All, GameState.Setup);

        //Make active player white
        TurnManager.Instance.photonView.RPC(nameof(TurnManager.Instance.RPCSetCurrentPlayer), RpcTarget.All, 1);

        while (!players[0].GetReserveCountOfPieceType(PieceType.Manok).Equals(0) || !players[1].GetReserveCountOfPieceType(PieceType.Manok).Equals(0))
        {
            yield return null;
        }

        UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCEnableEverythingElse), RpcTarget.All);
        TurnManager.Instance.photonView.RPC(nameof(TurnManager.Instance.RPCSetCurrentPlayer), RpcTarget.All, 2);
        UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCUpdateStateText), RpcTarget.All);

        this.photonView.RPC(nameof(SetCurrentGameState), RpcTarget.All, GameState.GoingOn);

        yield return null;
    }



    IEnumerator LocalSetupSequence()
    {
        while (players.Count != 2)
        {
            yield return null;
        }
        yield return null;
        yield return shortWait;


        _currentGameState = GameState.Setup;
        TurnManager.Instance.photonView.RPC(nameof(TurnManager.Instance.RPCSetCurrentPlayer), RpcTarget.All, 1);

        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            UIManager.Instance.SetPlayerHeaderTexts(i, GameManager.Instance.players[i]);
        }

        UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCUpdateStateText), RpcTarget.All);

        while (!players[0].GetReserveCountOfPieceType(PieceType.Manok).Equals(0) || !players[1].GetReserveCountOfPieceType(PieceType.Manok).Equals(0))
        {
            yield return null;
        }

        yield return shortWait;

        UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCEnableEverythingElse), RpcTarget.All);
        TurnManager.Instance.photonView.RPC(nameof(TurnManager.Instance.RPCSetCurrentPlayer), RpcTarget.All, 2);
        UIManager.Instance.photonView.RPC(nameof(UIManager.Instance.RPCUpdateStateText), RpcTarget.All);

        _currentGameState = GameState.GoingOn;
    }
}
/* if (PhotonNetwork.IsMasterClient)
 {
     while (players.Count != 2 && !NetworkManager.Instance.isPhotonOffline)
     {
         yield return null;
     }

     yield return shortWait;

     //TurnManager.Instance.Initialize(1);
     TurnManager.Instance.photonView.RPC("RPCSetPlayerColors", RpcTarget.All);
     TurnManager.Instance.photonView.RPC("RPCSetCurrentPlayer", RpcTarget.All, 1);*/

//foreach (Player player in players)
//  {
//     player.photonView.RPC("RPCSetTayogReserve", RpcTarget.All);
//     player.photonView.RPC("RPCGenerateTayogReserve", RpcTarget.All);
//player.RPCGenerateTayogReserve();
//}
/* Debug.LogError("master");

 foreach(Player player in players)
 {
     player.photonView.RPC("RPCSetTayogReserve", RpcTarget.All);
     player.photonView.RPC("RPCGenerateTayogReserve", RpcTarget.All);
     Debug.LogError("master2");
 }
 Debug.LogError("master worked");
 //InitialSetup();


 ///yield return shortWait;


 /* _currentGameState = GameState.Setup;
  UIManager.Instance.SetPlayerHeaderTexts();
  UIManager.Instance.UpdateStateText();

  while (!players[0].GetReserveCountOfPieceType(PieceType.Manok).Equals(0) || !players[1].GetReserveCountOfPieceType(PieceType.Manok).Equals(0))
  {
      yield return null;
  }

  yield return shortWait;

  UIManager.Instance.EnableEverythingElse();
  TurnManager.Instance.SetCurrentPlayer(2);
  UIManager.Instance.UpdateStateText();

  _currentGameState = GameState.GoingOn;
}

else
{
 Debug.LogError(photonView.ControllerActorNr.ToString());
 Debug.LogError(photonView.OwnerActorNr.ToString());
 Debug.LogError(photonView.CreatorActorNr.ToString());
 while (players.Count != 2 && !NetworkManager.Instance.isPhotonOffline)
 {
     yield return null;
 }

 yield return shortWait;

 //TurnManager.Instance.Initialize(1);
 TurnManager.Instance.photonView.RPC("RPCSetPlayerColors", RpcTarget.All);
 TurnManager.Instance.photonView.RPC("RPCSetCurrentPlayer", RpcTarget.All, 2);
 Debug.LogError("client");

 foreach (Player player in players)
 {
     player.photonView.RPC("RPCSetTayogReserve", RpcTarget.All);
     player.photonView.RPC("RPCGenerateTayogReserve", RpcTarget.All);
     Debug.LogError("client2");
 }
 Debug.LogError("client worked");
}*/

