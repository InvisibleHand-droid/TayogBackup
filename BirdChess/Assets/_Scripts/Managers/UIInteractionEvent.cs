using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UIEvent
{
    public UnityEvent Event;
    public float timeTillNextEvent;
}

public class UIInteractionEvent : MonoBehaviour
{
    public UIEvent[] HoverEvents;
    public UIEvent[] OnClickEvents;
}
