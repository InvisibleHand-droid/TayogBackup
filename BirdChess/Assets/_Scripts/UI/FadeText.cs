using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private float timeBeforeFade = 2;
    [SerializeField] private float fadeLength = 0.5f;
    
    public void StartFade()
    {
        StopAllCoroutines();
        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1);
        StartCoroutine(Fading());
    }
    private IEnumerator Fading()
    {
        float TBF = timeBeforeFade;
        while(TBF > 0)
        {
            TBF -= Time.deltaTime;
            yield return null;
        }
        
        float timePassed = 0;
        Color startColor = new Color(displayText.color.r, displayText.color.g, displayText.color.b, displayText.color.a);
        Color endColor = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0);
        while (timePassed < fadeLength)
        {
            displayText.color = Color.Lerp(startColor, endColor, timePassed/fadeLength);
            timePassed += Time.deltaTime;
            yield return null;
        }
        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0);
    }
}
