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
public class Player : MonoBehaviourPun
{
    public PlayerMove playerMove;
    public GameEventChannel gameEventChannel;
    public TeamColor _teamColor;
    public TayogSet _tayogSet;
    public List<TayogPiece> _reserveTayogPiece = new List<TayogPiece>();
    public List<TayogPiece> _activeTayogPiece = new List<TayogPiece>();

    //Change this later to playerprefs
    public string chosenWhiteSkinID;
    public string chosenBlackSkinID;

    public TayogPiece previouslyPlayedTayogPiece;

    public virtual void Awake()
    {
    }
    public virtual void Start()
    {
        playerMove = GetComponent<PlayerMove>();

    }

    [PunRPC]
    public void RPCSetTayogReserve()
    {
        string skinTarget = null;
        switch (_teamColor)
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
                _tayogSet = tayogSet;
                break;
            }
        }
    }

    [PunRPC]
    public void RPCGenerateTayogReserve()
    {
        Debug.Log("generated tayog");
        if (!this.photonView.IsMine) return;
        GameSettings gameSettings = GameManager.Instance.gameSettings;
        int amountToPool = 0;
        foreach (GeneratedTayogPiece generatedTayogPiece in _tayogSet.generatedTayogPieces)
        {
            string prefabPieceSetTarget = _tayogSet.ID;
            string prefabPieceTarget = generatedTayogPiece._tayogPiecePrefab.name;

            switch (generatedTayogPiece._pieceType)
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
                this.photonView.RPC(nameof(RPCGenerateTayogPiece), RpcTarget.All, prefabPieceSetTarget, prefabPieceTarget);
            }
        }
    }

    [PunRPC]
    public void RPCGenerateTayogPiece(string prefabPieceSetTarget, string prefabPieceTarget)
    {
        if (!this.photonView.IsMine) return;
        GameObject pooledPhotonTayogPiece;

        pooledPhotonTayogPiece = PhotonNetwork.Instantiate(Path.Combine("PiecePrefabs", prefabPieceSetTarget, prefabPieceTarget)
        , transform.position, Quaternion.identity);

        pooledPhotonTayogPiece.GetComponent<TayogPiece>().SetTeamColor(_teamColor);

        pooledPhotonTayogPiece.transform.parent = this.gameObject.transform;

        pooledPhotonTayogPiece.gameObject.SetActive(false);

        _reserveTayogPiece.Add(pooledPhotonTayogPiece.GetComponent<TayogPiece>());
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
            if (tayogPiece.GetPieceType() == PieceType.Manok)
            {
                manokActive++;
            }
        }

        return manokActive;
    }

    private void CheckIfIGotValidMoves()
    {

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
            Debug.Log(reservedTayogPiece);
            if (!reservedTayogPiece.gameObject.activeInHierarchy && reservedTayogPiece.GetPieceType().Equals(targetPieceType))
            {
                Debug.Log("returned" + reservedTayogPiece);
                return reservedTayogPiece;
            }
        }
        Debug.Log("returned null");
        return null;
    }

    public void ReconstructTayogPiece(TayogPiece tayogPiece)
    {
        TayogPiece tayogReference = null;
        foreach (GeneratedTayogPiece generatedTayogPiece in _tayogSet.generatedTayogPieces)
        {
            if (generatedTayogPiece._pieceType == tayogPiece.GetPieceType())
            {
                tayogReference = generatedTayogPiece._tayogPiecePrefab;
            }
        }

        tayogPiece.name = $"{_tayogSet.ID}{tayogReference.GetPieceType().ToString()}";
        tayogPiece.GetComponent<MeshFilter>().sharedMesh = tayogReference.GetComponent<MeshFilter>().sharedMesh;
        tayogPiece.GetComponent<Renderer>().sharedMaterial = tayogReference.GetComponent<Renderer>().sharedMaterial;
        tayogPiece.SetTeamColor(_teamColor);
        tayogPiece.SetPieceState(PieceState.Reserve);
        tayogPiece.transform.SetParent(this.transform);
    }
}
