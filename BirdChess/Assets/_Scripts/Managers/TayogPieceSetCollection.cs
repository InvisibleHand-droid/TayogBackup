using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TayogSet", menuName = "TayogElements/TayogSetCollection", order = 1)]
public class TayogPieceSetCollection : ScriptableGameObject
{
    public List<TayogPieceSet> tayogPieceSets = new List<TayogPieceSet>();
}