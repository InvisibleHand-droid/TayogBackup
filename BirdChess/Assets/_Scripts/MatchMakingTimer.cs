using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingTimer : Timer
{
    [SerializeField] private MainMenuUIManager mainMenuUIManager;
    // Update is called once per frame
    public override void Update()
    {
        if(isTimerOn)
        {
            base.Update();
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        mainMenuUIManager.timerText.SetText(GetTimerString());
    }

    public override void ResetTimer()
    {
        _timer = isIncrement ? 0 : (_minutes * 60) + _seconds;
        UpdateTimerText();
    }
}
