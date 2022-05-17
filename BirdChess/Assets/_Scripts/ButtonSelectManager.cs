using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelectManager : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _idleSprite;
    public GameObject activeButton;
    
    //Change to this later
    public void MakeButtonActive(Image image)
    {
        image.sprite = _selectedSprite;
    }
}
