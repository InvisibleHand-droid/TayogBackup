using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedTayogSprite
{
    public PieceType pieceType;
    public Sprite pieceSprite;
    public AnimatorOverrideController pieceAnimator;

    public AudioClip summonSound;
    public AudioClip selectSound;
    public AudioClip attackSound;
    public AudioClip captureSound;
}

[CreateAssetMenu(fileName = "TayogSpriteSet", menuName = "GameElements/TayogSpriteSet", order = 0)]
public class TayogSpriteSet : ScriptableGameObject
{
    public List<GeneratedTayogSprite> generatedTayogSpriteSetWhite = new List<GeneratedTayogSprite>();
    public List<GeneratedTayogSprite> generatedTayogSpriteSetBlack = new List<GeneratedTayogSprite>();
    public string ID;
    public Sprite indicator;
}
