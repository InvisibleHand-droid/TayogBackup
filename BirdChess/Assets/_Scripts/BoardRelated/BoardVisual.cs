using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardVisual", menuName = "Board/BoardVisual", order = 0)]
public class BoardVisual: ScriptableGameObject
{
    public GameObject boardPrefab;
    public string boardID;
    public AudioClip boardTheme;
    public Vector3 offset;
}