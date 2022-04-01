using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceManager : Singleton<PieceManager>
{
    public TayogPiece selectedTayogPiece;

    public void ButtonTakeFromReserve(ButtonReserveTarget buttonReserveTarget)
    {
        Player targetPlayerReserve = buttonReserveTarget.player;
        TayogPiece targetPooledTayogPiece = targetPlayerReserve.GetPooledTayogPiece(buttonReserveTarget.pieceType);
        
        if (targetPooledTayogPiece != null)
        {
            targetPooledTayogPiece.Select();
        }
    }
}
