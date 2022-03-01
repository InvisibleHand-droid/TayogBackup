using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileState
{
    Occupied,
    Empty
}
public class Tile : MonoBehaviour
{
    public Stack<TayogPiece> tayogPiecesAboveMe = new Stack<TayogPiece>();
    private MeshRenderer _meshRenderer;
    private Color _baseSpriteColor;
    public int rowID;
    public int columnID;
    public bool isValid;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        TileManager.Instance.allTiles.Add(this);
    }

    public void ReceiveSelectedTayogPiece(TayogPiece tayogPiece)
    {
        if (tayogPiece.GetPieceType() == PieceType.Manok)
        {
            if(tayogPiecesAboveMe.Count > 0){
                if (tayogPiece.gameObject.activeInHierarchy && tayogPiece.GetTeamColor() != GetTayogPieceOnTop().GetTeamColor())
                {
                    tayogPiece.Capture(this);
                }
            }
            else if (tayogPiece.gameObject.activeInHierarchy)
            {
                tayogPiece.Perch(this);
            }
            //Piece selected is from reserve, get its rally targets
            else if (!tayogPiece.gameObject.activeInHierarchy)
            {
                tayogPiece.Rally(this);
            }
        }
        else
        {
            if (tayogPiece.gameObject.activeInHierarchy && tayogPiece.GetTeamColor() != GetTayogPieceOnTop().GetTeamColor() && GetTayogPieceOnTop() != null)
            {
                tayogPiece.Capture(this);
            }
            else if (tayogPiece.gameObject.activeInHierarchy && tayogPiece.GetTeamColor() == GetTayogPieceOnTop().GetTeamColor() && GetTayogPieceOnTop() != null)
            {
                tayogPiece.Perch(this);
            }
            //Piece selected is from reserve, get its rally targets
            else if (!tayogPiece.gameObject.activeInHierarchy)
            {
                tayogPiece.Rally(this);
            }
        }

    }

    #region Tile Highlighting
    public void Highlight()
    {
        if (_meshRenderer.enabled == true) return;

        _meshRenderer.enabled = true;
    }

    public void RemoveHighlight()
    {
        if (_meshRenderer.enabled == false) return;

        _meshRenderer.enabled = false;
    }

    public void SetHighlightOfTayogPieceOnTop(bool value)
    {
        GetTayogPieceOnTop().SetHighlight(true);
    }
    #endregion

    public TayogPiece GetTayogPieceOnTop()
    {
        if (tayogPiecesAboveMe.Count <= 0) return null;
        return tayogPiecesAboveMe.Peek();
    }

    public Vector3 Top()
    {
        //Replace Count later with Mesh Bounds
        return new Vector3(this.transform.position.x, this.transform.position.y + (tayogPiecesAboveMe.Count * 0.25f), this.transform.position.z);
    }
}
