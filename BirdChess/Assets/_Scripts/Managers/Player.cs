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
    public TayogSet tayogSet;
    public List<TayogPiece> _reserveTayogPiece = new List<TayogPiece>();
    public List<TayogPiece> _activeTayogPiece = new List<TayogPiece>();

    //Change this later to playerprefs
    public string chosenWhiteSkinID;
    public string chosenBlackSkinID;

    public TayogPiece previouslyPlayedTayogPiece;

    public virtual void Start()
    {
        playerMove = GetComponent<PlayerMove>();

        if (this.photonView.IsMine)
        {
            this.photonView.RPC(nameof(SetColor), RpcTarget.All);

            this.photonView.RPC(nameof(RPCSetTayogReserve), RpcTarget.All);
            //this.photonView.RPC(nameof(RPCGenerateTayogReserve), RpcTarget.All);
            GenerateTayogReserve();
        }
    }

    public void InitializeUI()
    {
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
    public void RPCSetTayogReserve()
    {
        string skinTarget = null;
        switch (teamColor)
        {
            case (TeamColor.White):
                skinTarget = chosenWhiteSkinID;
                break;
            case (TeamColor.Black):
                skinTarget = chosenBlackSkinID;
                break;

        }

        foreach (TayogSet tayogSet in GameManager.Instance.tayogSetCollection.tayogSets)
        {
            if (tayogSet.ID == skinTarget)
            {
                this.tayogSet = tayogSet;
                break;
            }
        }
    }

    public void GenerateTayogReserve()
    {
        GameSettings gameSettings = GameManager.Instance.gameSettings;
        int amountToPool = 0;
        foreach (GeneratedTayogPiece generatedTayogPiece in tayogSet.generatedTayogPieces)
        {
            string prefabPieceSetTarget = tayogSet.ID;
            string prefabPieceTarget = generatedTayogPiece.tayogPiecePrefab.name;

            switch (generatedTayogPiece.pieceType)
            {
                case PieceType.Manok:
                    amountToPool = gameSettings.ManokCount;
                    break;
                case PieceType.Bibe:
                    amountToPool = gameSettings.BibeCount;
                    break;
                case PieceType.Lawin:
                    amountToPool = gameSettings.LawinCount;
                    break;
                case PieceType.Agila:
                    amountToPool = gameSettings.AgilaCount;
                    break;
            }

            for (int i = 0; i < amountToPool; i++)
            {
                // this.photonView.RPC(nameof(RPCGenerateTayogPiece), RpcTarget.All, prefabPieceSetTarget, prefabPieceTarget);
                object[] instanceData = new object[1];
                instanceData[0] = this.tag;

                Quaternion initialRotation = generatedTayogPiece.tayogPiecePrefab.transform.rotation;

                if (!PhotonNetwork.IsMasterClient)
                {
                    initialRotation = Quaternion.Euler(-90, 180, -180);
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

        foreach (TayogPiece tayogPiece in _reserveTayogPiece)
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

        foreach (TayogPiece tayogPiece in _activeTayogPiece)
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
        _activeTayogPiece.Remove(tayogPiece);
        _reserveTayogPiece.Add(tayogPiece);
    }

    public void AddToActiveList(TayogPiece tayogPiece)
    {
        _reserveTayogPiece.Remove(tayogPiece);
        _activeTayogPiece.Add(tayogPiece);
    }

    public TayogPiece GetPooledTayogPiece(PieceType targetPieceType)
    {
        foreach (TayogPiece reservedTayogPiece in _reserveTayogPiece)
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
        TayogPiece tayogReference = null;
        foreach (GeneratedTayogPiece generatedTayogPiece in tayogSet.generatedTayogPieces)
        {
            if (generatedTayogPiece.pieceType == tayogPiece.GetPieceType())
            {
                tayogReference = generatedTayogPiece.tayogPiecePrefab;
                tayogPiece.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = generatedTayogPiece.pieceSprite;
            }
        }

        tayogPiece.photonView.TransferOwnership(this.photonView.Controller);

        if (!PhotonNetwork.IsMasterClient)
        {
            Vector3 euler = tayogPiece.transform.eulerAngles;
            tayogPiece.transform.localEulerAngles = new Vector3(-90, 180, -90);
            //tayogPiece.transform.rotation = Quaternion.Euler(new Vector3(-90, 180, 180));
        }
        else
        {
            Vector3 euler = tayogPiece.transform.eulerAngles;
            tayogPiece.transform.localEulerAngles = new Vector3(-90, 180, 90);
        }

        tayogPiece.name = $"{tayogSet.ID}{tayogReference.GetPieceType().ToString()}";
        tayogPiece.GetComponent<MeshFilter>().sharedMesh = tayogReference.GetComponent<MeshFilter>().sharedMesh;
        tayogPiece.GetComponent<Renderer>().sharedMaterial = tayogReference.GetComponent<Renderer>().sharedMaterial;
        tayogPiece.SetTeamColor(teamColor);
        tayogPiece.SetPieceState(PieceState.Reserve);
        tayogPiece.transform.SetParent(this.transform);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
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
    }
}
