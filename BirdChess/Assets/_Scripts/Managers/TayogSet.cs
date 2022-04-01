using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedTayogPiece
{
    public TayogPiece tayogPiecePrefab;
    public PieceType pieceType;
    public AnimatorOverrideController pieceAnimator;
    public Sprite pieceSprite;
}

[CreateAssetMenu(fileName = "TayogSet", menuName = "GameElements/TayogSet", order = 0)]
public class TayogSet : ScriptableGameObject
{
    public List<GeneratedTayogPiece> generatedTayogPieces = new List<GeneratedTayogPiece>();
    public string ID;
}
