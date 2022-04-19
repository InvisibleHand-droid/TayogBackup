using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedTayogPiece
{
    public PieceType pieceType;
    public Mesh pieceMesh;
    public Material pieceMaterial;
    public TayogPiece tayogPiecePrefab;
}

[CreateAssetMenu(fileName = "TayogSet", menuName = "GameElements/TayogPieceSet", order = 0)]
public class TayogPieceSet : ScriptableGameObject
{
    public List<GeneratedTayogPiece> generatedTayogPiecesWhite = new List<GeneratedTayogPiece>();
    public List<GeneratedTayogPiece> generatedTayogPiecesBlack = new List<GeneratedTayogPiece>();

    public string ID;
}
