using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITayogRange
{
    public List<Tile> GetValidTiles();
    public List<Tile> GetValidRallyTiles();
    public List<Tile> GetValidCaptureOrPerchTiles();
}
