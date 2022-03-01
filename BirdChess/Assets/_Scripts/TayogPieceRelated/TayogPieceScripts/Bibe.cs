using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bibe : TayogPiece
{
    #region Booleans
    private bool checkUp;
    private bool checkDown;
    private bool checkLeft;
    private bool checkRight;
    #endregion
    
    #region Action Conditions
    public override bool CanCapture(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Manok))
         || (!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Bibe)));
    }

    public override bool CanPerch(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Manok)));
    }

    public override bool CanRally(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Manok)));
    }
    #endregion
    
    #region Range 
    public override List<Tile> GetValidCaptureOrPerchTiles()
    {
        //at the start you can check if every direction
        checkUp = true;
        checkDown = true;
        checkLeft = true;
        checkRight = true;

        List<Tile> validCaptureOrPerchTiles = new List<Tile>();

        //for each direction check every tile if you can add it
        for (int i = 0; i < 8; i++)
        {
            //up
            if (checkUp)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_UP, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow));
            }


            //down
            if (checkDown)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_DOWN, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow));
            }


            //left
            if (checkLeft)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_LEFT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn, _currentTileRow - i));
            }


            //right 
            if (checkRight)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_RIGHT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn, _currentTileRow + i));
            }
        }

        return validCaptureOrPerchTiles;
    }

    private void CheckIfTileCanBeAdded(string direction, List<Tile> list, Tile tile)
    {

        Tile tileTarget = tile;
        //if tile doesn't exist or there is nothing on top of the tile, return
        if (tileTarget == null || tileTarget.GetTayogPieceOnTop() == null) return;
        //if the piece on top is an enemy, and its a lawin or an agila, stop checking in that direction and return nothing
        if (tile.GetTayogPieceOnTop().GetTeamColor() != _pieceTeamColor &&(tile.GetTayogPieceOnTop().GetPieceType() == PieceType.Lawin || tile.GetTayogPieceOnTop().GetPieceType() == PieceType.Agila))
        {
            switch (direction)
            {
                case TayogRef.DIR_UP:
                    checkUp = false;
                    break;
                case TayogRef.DIR_DOWN:
                    checkDown = false;
                    break;
                case TayogRef.DIR_LEFT:
                    checkLeft = false;
                    break;
                case TayogRef.DIR_RIGHT:
                    checkRight = false;
                    break;
            }
            return;
        }
        //else check if you can perch or capture in that tile, if you can add it
        if (CanPerch(tileTarget) || CanCapture(tileTarget))
        {
            list.Add(tileTarget);
        }
    }
    #endregion
}
