using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardVisualsCollection", menuName = "Board/BoardVisualsCollection", order = 1)]
public class BoardVisualsCollection : ScriptableGameObject
{
    public List<BoardVisual> boardVisualsCollection = new List<BoardVisual>();
}
