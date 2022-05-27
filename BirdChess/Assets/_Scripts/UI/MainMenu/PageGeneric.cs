using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageGeneric : MonoBehaviour
{
    public void OpenPage()
    {
        gameObject.SetActive(true);
    }
    public void ClosePage()
    {
        gameObject.SetActive(false);
    }
}
