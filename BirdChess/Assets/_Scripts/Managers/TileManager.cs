using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    public List<Tile> allTiles = new List<Tile>();
    public Tile selectedTile;

    public override void Awake()
    {
        base.Awake();
    }
    public void ClearHighlights()
    {
        foreach(Tile tile in allTiles)
        {
            tile.RemoveHighlight();
            tile.isValid = false;
        }
    }

    public Tile GetTileBasedOnID(int column, int row)
    {
        foreach(Tile tile in allTiles){
            if(tile.columnID == column && tile.rowID == row)
            {
                return tile;
            }
        }
        return null;
    }
}
