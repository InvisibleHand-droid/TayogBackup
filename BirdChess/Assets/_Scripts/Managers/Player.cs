using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public enum TeamColor
{
    White = 0,
    Black = 1
}
public class Player : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PlayerMove playerMove;
    public GameEventChannel gameEventChannel;
    public TeamColor teamColor;
    public TayogPieceSet tayogPieceSet;
    public TayogSpriteSet tayogSpriteSet;
    public List<TayogPiece> reserveTayogPiece = new List<TayogPiece>();
    public List<TayogPiece> activeTayogPiece = new List<TayogPiece>();

    //Change this later to playerprefs
    public string chosenSpriteID;
    public string chosenPieceID;

    public TayogPiece previouslyPlayedTayogPiece;
    public List<GeneratedTayogPiece> skinTarget;
    public Timer timer;

    public virtual void Start()
    {
        playerMove = GetComponent<PlayerMove>();

        if (this.photonView.IsMine)
        {
            //Debug.LogError(chosenPieceID);
            //Debug.LogError(chosenSpriteID);
            this.photonView.RPC(nameof(SetColor), RpcTarget.All);
            this.photonView.RPC(nameof(SetTimer), RpcTarget.All);
            this.photonView.RPC(nameof(RPCSetTayogReserve), RpcTarget.All);
            GenerateTayogReserve();
        }
    }

    [PunRPC]
    public void SetColor()
    {
        if (this.tag == "Player1")
        {
            this.teamColor = TeamColor.White;
        }
        else
        {
            this.teamColor = TeamColor.Black;
        }

        this.name = this.teamColor + "Player" + this.photonView.CreatorActorNr;
    }

    [PunRPC]
    public void SetTimer()
    {
        if (this.tag == "Player1")
        {
            this.teamColor = TeamColor.White;
        }
        else
        {
            this.teamColor = TeamColor.Black;
        }

        this.name = this.teamColor + "Player" + this.photonView.CreatorActorNr;
    }

    [PunRPC]
    public void RPCSetTayogReserve()
    {
        foreach (TayogPieceSet tayogPieceSet in VisualsManager.Instance.tayogPieceSetCollection.tayogPieceSets)
        {
            if (tayogPieceSet.ID == chosenPieceID)
            {
                this.tayogPieceSet = tayogPieceSet;
                break;
            }
        }

        foreach (TayogSpriteSet tayogSpriteSet in VisualsManager.Instance.tayogSpriteSetCollection.tayogSpriteSets)
        {
            if (tayogSpriteSet.ID == chosenSpriteID)
            {
                this.tayogSpriteSet = tayogSpriteSet;
                break;
            }
        }

        switch (teamColor)
        {
            case (TeamColor.White):
                skinTarget = tayogPieceSet.generatedTayogPiecesWhite;
                break;
            case (TeamColor.Black):
                skinTarget = tayogPieceSet.generatedTayogPiecesBlack;
                break;
        }

    }

    public void GenerateTayogReserve()
    {
        GameSettingsData gameSettings = GameManager.Instance.gameSettingsData;
        int amountToPool = 0;
        string prefabPieceSetTarget = "Standard";//chosenPieceID;
        foreach (GeneratedTayogPiece generatedTayogPiece in skinTarget)
        {
            string prefabPieceTarget = generatedTayogPiece.tayogPiecePrefab.name;

            switch (generatedTayogPiece.pieceType)
            {
                case PieceType.Manok:
                    amountToPool = gameSettings.ManokReserve;
                    break;
                case PieceType.Bibe:
                    amountToPool = gameSettings.BibeReserve;
                    break;
                case PieceType.Lawin:
                    amountToPool = gameSettings.LawinReserve;
                    break;
                case PieceType.Agila:
                    amountToPool = gameSettings.AgilaReserve;
                    break;
            }

            for (int i = 0; i < amountToPool; i++)
            {
                object[] instanceData = new object[2];
                instanceData[0] = this.tag;

                Quaternion initialRotation = generatedTayogPiece.tayogPiecePrefab.transform.rotation;

                if (!PhotonNetwork.IsMasterClient)
                {
                    initialRotation = Quaternion.Euler(-90, 0, -180);
                }

                GameObject pooledPhotonTayogPiece = PhotonNetwork.Instantiate(Path.Combine("PiecePrefabs", prefabPieceSetTarget, prefabPieceTarget)
                 , transform.position, initialRotation, 0, instanceData);
            }
        }
    }

    private bool CheckIfNoMoreManok()
    {
        if (!GetManokActiveCount().Equals(0)) return false;

        return true;
    }

    private bool CheckIfIGot8EnemyManok()
    {
        if (!GetReserveCountOfPieceType(PieceType.Manok).Equals(8)) return false;

        return true;
    }

    public int GetReserveCountOfPieceType(PieceType pieceType)
    {
        int count = 0;

        foreach (TayogPiece tayogPiece in reserveTayogPiece)
        {
            if (tayogPiece.GetPieceType() == pieceType)
            {
                count++;
            }
        }

        return count;
    }

    public int GetManokActiveCount()
    {
        int manokActive = 0;

        foreach (TayogPiece tayogPiece in activeTayogPiece)
        {
            if (tayogPiece.GetPieceType() == PieceType.Manok && tayogPiece.isExposed())
            {
                manokActive++;
            }
        }

        return manokActive;
    }

    public void AddToReserveList(TayogPiece tayogPiece)
    {
        activeTayogPiece.Remove(tayogPiece);
        reserveTayogPiece.Add(tayogPiece);
    }

    public void AddToActiveList(TayogPiece tayogPiece)
    {
        reserveTayogPiece.Remove(tayogPiece);
        activeTayogPiece.Add(tayogPiece);
    }

    public TayogPiece GetPooledTayogPiece(PieceType targetPieceType)
    {
        foreach (TayogPiece reservedTayogPiece in reserveTayogPiece)
        {
            if (!reservedTayogPiece.gameObject.activeInHierarchy && reservedTayogPiece.GetPieceType().Equals(targetPieceType))
            {
                return reservedTayogPiece;
            }
        }
        return null;
    }

    public void ReconstructTayogPiece(TayogPiece tayogPiece)
    {
        tayogPiece.photonView.TransferOwnership(this.photonView.Controller);

        if (!PhotonNetwork.IsMasterClient)
        {
            Vector3 euler = tayogPiece.transform.eulerAngles;
            tayogPiece.transform.localEulerAngles = new Vector3(-90, 180, -90);
        }
        else
        {
            Vector3 euler = tayogPiece.transform.eulerAngles;
            tayogPiece.transform.localEulerAngles = new Vector3(-90, 180, 90);
        }

        tayogPiece.SetTeamColor(teamColor);

        tayogPiece.name = $"{tayogPieceSet.ID}{tayogPiece.GetTeamColor()}{tayogPiece.GetPieceType().ToString()}";
        tayogPiece.AssignSprite();
        tayogPiece.AssignMesh();
        tayogPiece.SetPieceState(PieceState.Reserve);
        tayogPiece.transform.SetParent(this.transform);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;

        MultiplayerPlayerManager.playerCount++;
        GameManager.Instance.players.Add(this);
        if (PhotonNetwork.OfflineMode)
        {
            this.tag = "Player" + MultiplayerPlayerManager.playerCount.ToString();
        }
        else
        {
            this.tag = "Player" + this.photonView.CreatorActorNr;
        }

        chosenSpriteID = data[0].ToString();
        chosenPieceID = data[1].ToString();
    }
}
