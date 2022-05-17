using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private bool isToggled;
    [SerializeField] private UnityEvent ToggleOnEvents;
    [SerializeField] private UnityEvent ToggleOffEvents;
 
    public void Toggle()
    {
        isToggled = !isToggled;

        if(isToggled)
        {
            ToggleOffEvents.Invoke();
        }
        else
        {
            ToggleOnEvents.Invoke();
        }
    }
}
