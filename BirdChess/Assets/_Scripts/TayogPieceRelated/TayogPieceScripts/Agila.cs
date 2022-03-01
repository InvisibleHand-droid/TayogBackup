using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agila : TayogPiece
{
    #region Action Conditions
    public override bool CanCapture(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Agila))
         || (!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Lawin)));
    }

    public override bool CanPerch(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Agila)));
    }

    public override bool CanRally(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Lawin)));
    }
    #endregion

    #region Range
    public override List<Tile> GetValidCaptureOrPerchTiles()
    {
        List<Tile> validCaptureOrPerchTiles = new List<Tile>();

        //for each direction check every tile if you can add it
        for (int i = 1; i < 8; i++)
        {
            //up
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow));

            //down
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow));

            //left
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn, _currentTileRow - i));

            //right 
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn, _currentTileRow + i));

            //top right
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow + i));

            //top left
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow + i));

            //bottom right
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow - i));

            //bottom left
            CheckIfTileCanBeAdded(validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow - i));
        }

        return validCaptureOrPerchTiles;
    }

    private void CheckIfTileCanBeAdded(List<Tile> list, Tile tile)
    {

        Tile tileTarget = tile;

        //if tile doesn't exist return
        if (tileTarget == null) return;

        //if the piece on top is an enemy, and its a lawin or an agila, stop checking in that direction and return nothing
        if (CanPerch(tileTarget) || CanCapture(tileTarget))
        {
            list.Add(tileTarget);
        }
    }
    #endregion
}
