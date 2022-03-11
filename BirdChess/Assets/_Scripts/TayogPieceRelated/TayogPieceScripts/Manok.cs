using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manok : TayogPiece
{
    #region Action Conditions
    public override bool CanCapture(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null || GameManager._currentGameState == GameState.Setup) return false;
        return ((!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Manok)));
    }

    public override bool CanPerch(Tile tile)
    {
        return (tile.tayogPiecesAboveMe.Count <= 0);
    }

    public override bool CanRally(Tile tile)
    {
        return (tile.tayogPiecesAboveMe.Count <= 0);
    }
    #endregion

    #region Range
    public override List<Tile> GetValidCaptureOrPerchTiles()
    {
        List<Tile> validCaptureOrPerchTiles = new List<Tile>();
        //if the game hasn't started yet just return the empty list
        if (GameManager._currentGameState != GameState.GoingOn) return validCaptureOrPerchTiles;

        Tile currentTile = TileManager.Instance.GetTileBasedOnID(_currentTileColumn, _currentTileRow);

        //Adds empty squares around manok to move to
        foreach (Tile tile in GetSurroundingTiles("CaptureOrPerch", currentTile))
        {
            validCaptureOrPerchTiles.Add(tile);
        }

        return validCaptureOrPerchTiles;
    }

    public override List<Tile> GetValidRallyTiles()
    {
        List<Tile> validRallyTiles = new List<Tile>();
        int startingRow = 0;

        if (GameManager._currentGameState == GameState.Setup)
        {
            switch (_assignedPlayer.tag)
            {
                case "Player1":
                    startingRow = 0;
                    break;
                case "Player2":
                    startingRow = 7;
                    break;
            }

            foreach (Tile tile in TileManager.Instance.allTiles)
            {
                if (_assignedPlayer.GetManokActiveCount() < 1)
                {
                    if (tile.rowID == startingRow && tile.tayogPiecesAboveMe.Count <= 0)
                    {
                        validRallyTiles.Add(tile);
                    }
                }
            }
        }

        foreach (Tile tile in TileManager.Instance.allTiles)
        {
            if (tile.GetTayogPieceOnTop() != null)
            {
                if (_pieceTeamColor == tile.GetTayogPieceOnTop().GetTeamColor() && tile.GetTayogPieceOnTop().GetPieceType() == PieceType.Manok)
                {
                    //Check the surrounding of the tile if its empty make it valid
                    foreach (Tile surroundingTile in GetSurroundingTiles("Rally", tile))
                    {
                        validRallyTiles.Add(surroundingTile);
                    }
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

    private List<Tile> GetSurroundingTiles(string action, Tile tile)
    {
        List<Tile> surroundingManokTiles = new List<Tile>();

        //up
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID + 1, tile.rowID));

        //down
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID - 1, tile.rowID));

        //left
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID, tile.rowID - 1));

        //right 
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID, tile.rowID + 1));

        //top right
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID + 1, tile.rowID + 1));

        //top left
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID - 1, tile.rowID + 1));

        //bottom right
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID + 1, tile.rowID - 1));

        //bottom left
        CheckIfTileCanBeAdded(action, surroundingManokTiles, TileManager.Instance.GetTileBasedOnID(tile.columnID - 1, tile.rowID - 1));

        return surroundingManokTiles;
    }

    private void CheckIfTileCanBeAdded(string action, List<Tile> list, Tile tile)
    {

        Tile tileTarget = tile;

        if (tileTarget == null) return;
        switch (action)
        {
            case "CaptureOrPerch":
                if (CanCapture(tile) || CanPerch(tile))
                {
                    list.Add(tileTarget);
                }
                break;
            case "Rally":
                if (CanRally(tile))
                {
                    list.Add(tileTarget);
                }
                break;
        }
    }
    #endregion
}
