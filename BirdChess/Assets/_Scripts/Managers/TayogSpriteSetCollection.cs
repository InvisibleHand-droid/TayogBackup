using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TayogSet", menuName = "TayogElements/TayogSpriteSetCollection", order = 1)]
public class TayogSpriteSetCollection : ScriptableGameObject
{
    public List<TayogSpriteSet> tayogSpriteSets = new List<TayogSpriteSet>();
}
