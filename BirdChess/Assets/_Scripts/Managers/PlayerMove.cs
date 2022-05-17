using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun
{
    private Player _player;
    public bool isMyTurn;

    void Start()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (!isMyTurn || GameManager.currentGameState == GameState.End) return;
        HoverTile();

        if (Input.GetMouseButtonDown(1))
        {
            PieceManager.Instance.selectedTayogPiece = null;
            TileManager.Instance.ClearHighlights();
        }

    }

    public void MoveTo(Tile selectedTile, TayogPiece selectedTayogPiece)
    {
        selectedTile.ReceiveSelectedTayogPiece(selectedTayogPiece);

        //Clear Selections
        PieceManager.Instance.selectedTayogPiece = null;
        TileManager.Instance.selectedTile = null;

        //Go Next Turn
        TileManager.Instance.ClearHighlights();
    }

    public void HoverTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, LayerMask.GetMask("Tile")))
        {
            Tile hoveredTile = hit.transform.gameObject.GetComponent<Tile>();
            TileManager.Instance.selectedTile = hoveredTile;

            if (Input.GetMouseButtonDown(0))
            {
                if (PieceManager.Instance.selectedTayogPiece != null && TileManager.Instance.selectedTile != null)
                {
                    if (TileManager.Instance.selectedTile.GetTayogPieceOnTop() == PieceManager.Instance.selectedTayogPiece ||
                    TileManager.Instance.selectedTile.isValid.Equals(false))
                    {
                        TileManager.Instance.ClearHighlights();
                        PieceManager.Instance.selectedTayogPiece = null;
                    }
                    else
                    {
                        MoveTo(TileManager.Instance.selectedTile, PieceManager.Instance.selectedTayogPiece);
                    }
                }
                else
                {
                    SelectTayogPiece();
                }
            }
        }
        else
        {
           TileManager.Instance.selectedTile = null;
        }
    }

    public void SelectTayogPiece()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, LayerMask.GetMask("Tile")))
        {
            Tile hoveredTile = hit.transform.gameObject.GetComponent<Tile>();

            if (hoveredTile != null && hoveredTile.tayogPiecesAboveMe.Count > 0)
            {
                hoveredTile.GetTayogPieceOnTop().Select();
            }
        }
        else
        {
            TileManager.Instance.selectedTile = null;
        }
    }
}
