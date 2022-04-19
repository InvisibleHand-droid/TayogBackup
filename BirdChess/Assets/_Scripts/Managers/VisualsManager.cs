using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsManager : SingletonPersistent<VisualsManager>
{
    public TayogPieceSetCollection tayogPieceSetCollection;
    public TayogSpriteSetCollection tayogSpriteSetCollection;
    public BoardVisualsCollection boardVisualsCollection;
    public string chosenBoardID;
    [SerializeField] private List<string> _tayogSpriteSetIDs = new List<string>();
    [SerializeField] private List<string> _tayogPieceSetIDs = new List<string>();

    private void Start()
    {
        StoreIDs();

        if(!PlayerPrefs.HasKey("SpriteID"))
        {
            Debug.LogError("Set Sprite skin to Standard");
            PlayerPrefs.SetString("SpriteID", "Standard");
        }

        if(!PlayerPrefs.HasKey("PieceID"))
        {
            Debug.LogError("Set Piece Skin to Standard");
            PlayerPrefs.SetString("PieceID", "Standard");
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
    }

    public void SetSpriteSkin(string ID)
    {
        if (_tayogSpriteSetIDs.Contains(ID))
        {
            PlayerPrefs.SetString("SpriteID", ID);
            Debug.LogError($"{ID} is set.");
        }
        else
        {
            Debug.LogError("Sprite ID was Invalid. Standard is set.");
        }
    }

    public void SetPieceSkin(string ID)
    {
        if (_tayogPieceSetIDs.Contains(ID))
        {
            PlayerPrefs.SetString("PieceID", ID);
            Debug.LogError($"{ID} is set.");
        }
        else
        {
            Debug.LogError("Piece ID was Invalid. Standard is set.");
        }
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
    }

}
