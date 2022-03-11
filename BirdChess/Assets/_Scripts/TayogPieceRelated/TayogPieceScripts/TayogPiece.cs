using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public enum PieceState
{
    Reserve,
    Selected,
    Placed
}

public enum PieceType
{
    Manok = 0,
    Bibe = 1,
    Lawin = 2,
    Agila = 3
}
public abstract class TayogPiece : MonoBehaviourPun, ITayogMove, ITayogRange, IPunInstantiateMagicCallback
{
    #region Serialized Properties
    [SerializeField] protected TeamColor _pieceTeamColor;
    [SerializeField] protected PieceType _piecetype;
    [SerializeField] protected PieceState _pieceState;
    #endregion

    #region Tile ID
    protected int _currentTileColumn;
    protected int _currentTileRow;
    #endregion
    protected Player _assignedPlayer;
    protected Tile _assignedTile;
    private Color _baseColor;

    private void Awake()
    {
        _assignedPlayer = TurnManager.Instance.GetPlayerReferenceBasedOnColor(_pieceTeamColor);
        StoreColor();
    }

    #region Abstract action conditions
    public abstract bool CanCapture(Tile tile);
    public abstract bool CanPerch(Tile tile);
    public abstract bool CanRally(Tile tile);
    #endregion

    #region Private fields accessors
    public virtual void SetTeamColor(TeamColor teamColor)
    {
        _pieceTeamColor = teamColor;
    }
    public virtual void SetPieceState(PieceState pieceState)
    {
        _pieceState = pieceState;
    }
    public virtual TeamColor GetTeamColor()
    {
        return _pieceTeamColor;
    }
    public virtual PieceState GetPieceState()
    {
        return _pieceState;
    }
    public virtual PieceType GetPieceType()
    {
        return _piecetype;
    }
    #endregion

    #region Select Control
    //Select this tayog piece
    public void Select()
    {
        if (!isMoveable() || TurnManager.Instance.GetCurrentPlayer().previouslyPlayedTayogPiece == this ||
        GameManager._currentGameState == GameState.End) return;
        PieceManager.Instance.selectedTayogPiece = this;

        foreach (Tile tile in PieceManager.Instance.selectedTayogPiece.GetValidTiles())
        {
            tile.Highlight();
            tile.isValid = true;
        }
    }

    private bool isMoveable()
    {
        return _pieceTeamColor == TurnManager.Instance.GetCurrentPlayer()._teamColor;
    }
    #endregion

    #region Color Control
    //Store original material color reference
    private void StoreColor()
    {
        _baseColor = this.GetComponent<Renderer>().material.color;
    }

    //Restore original material color
    private void RemoveHighlight()
    {
        this.GetComponent<Renderer>().material.color = _baseColor;
    }

    private void Highlight()
    {
        if (this._pieceTeamColor == TeamColor.Black)
        {
            this.GetComponent<Renderer>().material.color = Color.black;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void SetHighlight(bool value)
    {
        if (value == true)
        {
            Highlight();
        }
        else
        {
            RemoveHighlight();
        }
    }
    #endregion

    #region Piece Actions
    public virtual void Capture(Tile tile)
    {
        this.photonView.RPC(nameof(RPCCapture), RpcTarget.All, tile.columnID, tile.rowID);

        //Update UI (ex. Played manok, -1 manok text ui)
        UIManager.Instance.photonView.RPC("RPCUpdatePlayerReserveText", RpcTarget.All);

        //Go next turn
        TurnManager.Instance.photonView.RPC("RPCNextTurn", RpcTarget.All);
    }
    public virtual void Perch(Tile tile)
    {
        this.photonView.RPC(nameof(RPCPerch), RpcTarget.All, tile.columnID, tile.rowID);
        //Update UI (ex. Played manok, -1 manok text ui)
        UIManager.Instance.photonView.RPC("RPCUpdatePlayerReserveText", RpcTarget.All);

        //Go next turn
        TurnManager.Instance.photonView.RPC("RPCNextTurn", RpcTarget.All);
    }
    public virtual void Rally(Tile tile)
    {
        this.photonView.RPC(nameof(RPCRally), RpcTarget.All, tile.columnID, tile.rowID);

        //Update UI (ex. Played manok, -1 manok text ui)
        UIManager.Instance.photonView.RPC("RPCUpdatePlayerReserveText", RpcTarget.All);

        //Go next turn
        TurnManager.Instance.photonView.RPC("RPCNextTurn", RpcTarget.All);
    }

    [PunRPC]
    public void RPCPerch(int columnID, int rowID)
    {
        Tile tile = TileManager.Instance.GetTileBasedOnID(columnID, rowID);
        //Pop self from previous assigned tile because the piece moved
        _assignedTile.tayogPiecesAboveMe.Pop();

        //Assign piece to new tile
        AssignSelectedPieceToTile(tile);
    }
    [PunRPC]
    public void RPCCapture(int columnID, int rowID)
    {
        Tile tile = TileManager.Instance.GetTileBasedOnID(columnID, rowID);
        //Pop self from previous assigned tile because the piece moved
        _assignedTile.tayogPiecesAboveMe.Pop();

        CaptureAllPiecesOnTop(tile);

        //Assign capturer to this tile
        AssignSelectedPieceToTile(tile);
    }

    [PunRPC]
    public void RPCRally(int columnID, int rowID)
    {
        Tile tile = TileManager.Instance.GetTileBasedOnID(columnID, rowID);
        //From the object pool (inactive), make it active
        this.gameObject.SetActive(true);

        //Move it out of the reserve list
        TurnManager.Instance.GetCurrentPlayer()._reserveTayogPiece.Remove(this);

        //Move it to active list
        TurnManager.Instance.GetCurrentPlayer()._activeTayogPiece.Add(this);

        //Assign it to a tile
        AssignSelectedPieceToTile(tile);
    }

    private void AssignSelectedPieceToTile(Tile tile)
    {
        this._pieceState = PieceState.Placed;

        this.transform.SetParent(tile.transform);

        this.transform.position = tile.Top();

        this._currentTileColumn = tile.columnID;
        this._currentTileRow = tile.rowID;
        this._assignedTile = tile;

        if (this._assignedPlayer.previouslyPlayedTayogPiece != null)
        {
            this._assignedPlayer.previouslyPlayedTayogPiece.RemoveHighlight();
        }

        this._assignedPlayer.previouslyPlayedTayogPiece = this;

        SetHighlight(true);

        //..And add new piece to the tile's stack
        tile.tayogPiecesAboveMe.Push(this);
    }

    private void CaptureAllPiecesOnTop(Tile tile)
    {
        int countOnCapture = tile.tayogPiecesAboveMe.Count;

        //Remove the captured piece from the active list of its assigned player and assign it to the capturer      
        for (int i = 0; i < countOnCapture; i++)
        {
            TayogPiece tayogPieceOnTop = tile.GetTayogPieceOnTop();
            tayogPieceOnTop._assignedPlayer._activeTayogPiece.Remove(tayogPieceOnTop);
            tayogPieceOnTop._assignedPlayer = TurnManager.Instance.GetCurrentPlayer();

            tayogPieceOnTop._assignedPlayer.ReconstructTayogPiece(tayogPieceOnTop);
            tayogPieceOnTop._assignedPlayer._reserveTayogPiece.Add(tayogPieceOnTop);
            tayogPieceOnTop.StoreColor();

            tayogPieceOnTop.transform.SetParent(_assignedPlayer.transform);
            tile.tayogPiecesAboveMe.Pop();
            tayogPieceOnTop.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Range
    public virtual List<Tile> GetValidTiles()
    {
        if (_pieceState == PieceState.Reserve)
        {
            return GetValidRallyTiles();
        }
        else if (_pieceState == PieceState.Placed)
        {
            return GetValidCaptureOrPerchTiles();
        }

        return null;
    }

    public virtual List<Tile> GetValidRallyTiles()
    {
        List<Tile> validRallyTiles = new List<Tile>();

        foreach (Tile tile in TileManager.Instance.allTiles)
        {
            if (tile.GetTayogPieceOnTop() != null)
            {
                if (CanRally(tile))
                {
                    //Check the surrounding of the tile if its empty make it valid
                    validRallyTiles.Add(tile);
                }
            }
        }

        for (int i = 0; i < validRallyTiles.Count; i++)
        {
            if (validRallyTiles[i] == null)
            {
                validRallyTiles.Remove(validRallyTiles[i]);
            }
        }

        return validRallyTiles;
    }

    public abstract List<Tile> GetValidCaptureOrPerchTiles();

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;

        string playerTargetTag = (string)data[0];
        this._assignedPlayer = GameObject.FindGameObjectWithTag(playerTargetTag).GetComponent<Player>();

        this.transform.SetParent(_assignedPlayer.transform);

        this.gameObject.SetActive(false);
        this._assignedPlayer._reserveTayogPiece.Add(this);
    }
    #endregion


}
