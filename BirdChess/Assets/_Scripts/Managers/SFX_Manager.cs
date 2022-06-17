using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip genericBtn;

    public void Play_GenericBtn()
    {
        audioSource.clip = genericBtn;
        audioSource.Play();
    }
}
