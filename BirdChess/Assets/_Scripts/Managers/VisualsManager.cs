using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsManager : SingletonPersistent<VisualsManager>
{
    public TayogPieceSetCollection tayogPieceSetCollection;
    public TayogSpriteSetCollection tayogSpriteSetCollection;
    public BoardVisualsCollection boardVisualsCollection;
    [SerializeField] private List<string> _tayogSpriteSetIDs = new List<string>();
    [SerializeField] private List<string> _tayogPieceSetIDs = new List<string>();
    [SerializeField] private List<string> _tayogBoardIDs = new List<string>();

    private void Start()
    {
        StoreIDs();

        if(!PlayerPrefs.HasKey(TayogRef.SPRITE_ID))
        {
            Debug.LogError("Set Sprite skin to Standard");
            PlayerPrefs.SetString(TayogRef.SPRITE_ID, TayogRef.STANDARD);
        }

        if(!PlayerPrefs.HasKey(TayogRef.PIECE_ID))
        {
            Debug.LogError("Set Piece Skin to Standard");
            PlayerPrefs.SetString(TayogRef.PIECE_ID, TayogRef.STANDARD);
        }

        if(!PlayerPrefs.HasKey(TayogRef.BOARD_ID))
        {
            Debug.LogError("Set Piece Skin to Standard");
            PlayerPrefs.SetString(TayogRef.PIECE_ID, TayogRef.STANDARD);
        }
    }

    private void StoreIDs()
    {
        foreach (TayogSpriteSet tayogSpriteSet in tayogSpriteSetCollection.tayogSpriteSets)
        {
            if (!_tayogSpriteSetIDs.Contains(tayogSpriteSet.ID))
            {
                _tayogSpriteSetIDs.Add(tayogSpriteSet.ID);
            }
        }

        foreach (TayogPieceSet tayogPieceSet in tayogPieceSetCollection.tayogPieceSets)
        {
            if (!_tayogPieceSetIDs.Contains(tayogPieceSet.ID))
            {
                _tayogPieceSetIDs.Add(tayogPieceSet.ID);
            }
        }

        foreach (BoardVisual boardVisual in boardVisualsCollection.boardVisualsCollection)
        {
            if (!_tayogBoardIDs.Contains(boardVisual.boardID))
            {
                _tayogPieceSetIDs.Add(boardVisual.boardID);
            }
        }
    }

    public void SetSpriteSkin(string ID)
    {
        if (_tayogSpriteSetIDs.Contains(ID))
        {
            PlayerPrefs.SetString(TayogRef.SPRITE_ID, ID);
            Debug.LogError($"{ID} is set.");
        }
        else
        {
            PlayerPrefs.SetString(TayogRef.SPRITE_ID, TayogRef.STANDARD);
            Debug.LogError("Sprite ID was Invalid. Standard is set.");
        }
    }

    public void SetPieceSkin(string ID)
    {
        if (_tayogPieceSetIDs.Contains(ID))
        {
            PlayerPrefs.SetString(TayogRef.PIECE_ID, ID);
            Debug.LogError($"{ID} is set.");
        }
        else
        {
            PlayerPrefs.SetString(TayogRef.PIECE_ID, TayogRef.STANDARD);
            Debug.LogError("Piece ID was Invalid. Standard is set.");
        }
    }

    public void SetBoardSkin(string ID)
    {
        if (_tayogPieceSetIDs.Contains(ID))
        {
            PlayerPrefs.SetString(TayogRef.BOARD_ID, ID);
            Debug.LogError($"{ID} Board is set.");
        }
        else
        {
            PlayerPrefs.SetString(TayogRef.BOARD_ID, TayogRef.STANDARD);
            Debug.LogError("Board ID was Invalid. Standard is set.");
        }
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
    }

}
