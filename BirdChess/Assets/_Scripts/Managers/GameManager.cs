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
    public GameObject boardParent;

    public List<Player> players = new List<Player>();
    WaitForSeconds shortWait = new WaitForSeconds(1f);
    //win conditions
    //at the start of player's turn, if they can't make any action/move, opponent win
    //when a piece is added to reserve, count Manok, if 8, player win
    //at the end of a player's action/move, if no Manok of the opponents send their headcount, player win
    public static GameState currentGameState;
    public override void Awake()
    {
        base.Awake();
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
        currentGameState = gameState;
    }

    public void CheckPlayerVictoryConditions()
    {
        if (GameManager.currentGameState != GameState.GoingOn) return;

        foreach (Player player in players)
        {
            if (player.GetReserveCountOfPieceType(PieceType.Manok) >= 8)
            {
                UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, player.teamColor, "win");
                currentGameState = GameState.End;
                break;
            }

            if (player.GetManokActiveCount() <= 0)
            {
                UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, player.teamColor, "lose");
                currentGameState = GameState.End;
                break;
            }
        }

        if (NoValidMoves())
        {
            UIManager.Instance.photonView.RPC("EnableVictoryWindow", RpcTarget.All, TurnManager.Instance.GetCurrentPlayer(), "has no valid moves");
            currentGameState = GameState.End;
        }
    }

    public bool NoValidMoves()
    {
        Player player = TurnManager.Instance.GetCurrentPlayer();
        Debug.Log(player + " turn start");
        List<TayogPiece> allTayogPiece = new List<TayogPiece>();

        bool hasNoMoves = false;
        foreach (TayogPiece reserveTayogPiece in player.reserveTayogPiece)
        {
            allTayogPiece.Add(reserveTayogPiece);
        }

        foreach (TayogPiece activeTayogPiece in player.activeTayogPiece)
        {
            allTayogPiece.Add(activeTayogPiece);
        }

        foreach (TayogPiece tayogPiece in allTayogPiece)
        {
            if (player.previouslyPlayedTayogPiece != tayogPiece)
            {
                if (tayogPiece.GetValidTiles().Count >= 0)
                {
                    hasNoMoves = false;
                }
                else
                {
                    hasNoMoves = true;
                }
            }
        }

        return hasNoMoves;
    }

    IEnumerator OnlineSetupSequence()
    {
        foreach (BoardVisual boardVisual in VisualsManager.Instance.boardVisualsCollection.boardVisualsCollection)
        {
            if (boardVisual.boardID == PlayerPrefs.GetString(TayogRef.BOARD_ID))
            {
                Instantiate(boardVisual.boardPrefab, boardParent.transform.position, Quaternion.identity);
                break;
            }
        }
        Player localPlayer = null;
        Player nonLocalPlayer = null;
        while (players.Count != 2)
        {
            yield return null;
        }

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
        foreach (BoardVisual boardVisual in VisualsManager.Instance.boardVisualsCollection.boardVisualsCollection)
        {
            if (boardVisual.boardID == PlayerPrefs.GetString(TayogRef.BOARD_ID))
            {
                Instantiate(boardVisual.boardPrefab, boardParent.transform.position, Quaternion.identity);
                break;
            }
        }

        while (players.Count != 2)
        {
            yield return null;
        }
        yield return null;
        yield return shortWait;


        currentGameState = GameState.Setup;
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



        currentGameState = GameState.GoingOn;
    }
}

