using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TayogSet", menuName = "TayogElements/TayogSetCollection", order = 1)]
public class TayogSetCollection : ScriptableGameObject
{
    public List<TayogSet> tayogSets = new List<TayogSet>();
}