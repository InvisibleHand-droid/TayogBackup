using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSettings")]
//Change to this later maybe
public class GameSettingsData : ScriptableGameObject
{
    [Header("Reserve Count")]
    public int ManokReserve;
    public int BibeReserve;
    public int LawinReserve;
    public int AgilaReserve;

    [Header("Timer")]
    public int minutes;
    public int seconds;
}
