using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITayogMove
{
    public void Perch(Tile tile);
    public void Capture(Tile tile);
    public void Rally(Tile tile);
}
