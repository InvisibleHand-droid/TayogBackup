using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEventChannel gameEventChannel;
    public UnityEvent response;
    private void OnEnable()
    {
        gameEventChannel.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEventChannel.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
