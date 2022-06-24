using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkingProfileManager : MonoBehaviour
{

    private static NetworkingProfileManager _instance;

    public static NetworkingProfileManager instance { get { return _instance; } }

    public string baseURL = @"http://127.0.0.1:8000/api/player/";

    public static CmsPlayerProfile loggedProfile;

    // REGISTER METHODS
    public InputField nameInputField;
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

}
