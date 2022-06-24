using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmsManager : MonoBehaviour
{
    private static CmsManager _instance;

    public static CmsManager instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void EnablePlayerBoolean(string cms_variable)
    {
        // 
    }
}
