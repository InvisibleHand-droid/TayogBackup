using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerScript : Player
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        GameManager.Instance.players.Add(this);
    }
}
