using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip homeBGM;
    [SerializeField] private AudioClip homeBGM_Loop;
    private IEnumerator homeBGMLoop;

    private void Start()
    {
        PlayHomeBGM();
    }

    private void PlayHomeBGM()
    {
        audioSource.loop = false;
        audioSource.clip = homeBGM;
        audioSource.Play();
        homeBGMLoop = HomeBGMLoop();
        StartCoroutine(homeBGMLoop);
    }

    private IEnumerator HomeBGMLoop()
    {
        //note: easily missable transition hiccup, may have to adjust the time to play the loop version abit earlier.
        while(audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = homeBGM_Loop;
        audioSource.Play();
        audioSource.loop = true;
    }
}
