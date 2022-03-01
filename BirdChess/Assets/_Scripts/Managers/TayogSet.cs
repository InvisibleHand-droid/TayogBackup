using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedTayogPiece
{
    public TayogPiece _tayogPiecePrefab;
    public PieceType _pieceType;
}

[CreateAssetMenu(fileName = "TayogSet", menuName = "GameElements/TayogSet", order = 0)]
public class TayogSet : ScriptableGameObject
{
    public List<GeneratedTayogPiece> generatedTayogPieces = new List<GeneratedTayogPiece>();
    public string ID;
}
