using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawin : TayogPiece
{
    #region Booleans
    private bool checkTopRight;
    private bool checkTopLeft;
    private bool checkBottomLeft;
    private bool checkBottomRight;
    #endregion

    #region Action Conditions
    public override bool CanCapture(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Bibe))
         || (!_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Lawin)));
    }

    public override bool CanPerch(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Bibe)));
    }

    public override bool CanRally(Tile tile)
    {
        TayogPiece targetTayogPiece = tile.GetTayogPieceOnTop();
        if (targetTayogPiece == null) return false;
        return ((_pieceTeamColor.Equals(targetTayogPiece.GetTeamColor()) && targetTayogPiece.GetPieceType().Equals(PieceType.Bibe)));
    }
    #endregion

    #region Range
    public override List<Tile> GetValidCaptureOrPerchTiles()
    {
        checkTopRight = true;
        checkBottomLeft = true;
        checkTopLeft = true;
        checkBottomRight = true;
        List<Tile> validCaptureOrPerchTiles = new List<Tile>();

        //diagonal
        for (int i = 0; i < 8; i++)
        {
            //top right
            if (checkTopRight)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_TOPRIGHT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow + i));
            }


            //top left
            if (checkTopLeft)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_TOPLEFT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow + i));
            }


            //bottom right
            if (checkBottomRight)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_BOTTOMRIGHT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn + i, _currentTileRow - i));
            }


            //bottom left
            if (checkBottomLeft)
            {
                CheckIfTileCanBeAdded(TayogRef.DIR_BOTTOMLEFT, validCaptureOrPerchTiles, TileManager.Instance.GetTileBasedOnID(_currentTileColumn - i, _currentTileRow - i));
            }

        }

        return validCaptureOrPerchTiles;
    }

    private void CheckIfTileCanBeAdded(string direction, List<Tile> list, Tile tile)
    {

        Tile tileTarget = tile;

        if (tileTarget == null || tileTarget.GetTayogPieceOnTop() == null) return;

        if (tileTarget.GetTayogPieceOnTop().GetTeamColor() != _pieceTeamColor && tileTarget.GetTayogPieceOnTop().GetPieceType() == PieceType.Agila)
        {
            switch (direction)
            {
                case TayogRef.DIR_TOPRIGHT:
                    checkTopRight = false;
                    break;
                case TayogRef.DIR_TOPLEFT:
                    checkTopLeft = false;
                    break;
                case TayogRef.DIR_BOTTOMLEFT:
                    checkBottomLeft = false;
                    break;
                case TayogRef.DIR_BOTTOMRIGHT:
                    checkBottomRight = false;
                    break;
            }
            return;
        }

        if (CanPerch(tileTarget) || CanCapture(tileTarget))
        {
            list.Add(tileTarget);
        }
    }
    #endregion
}
