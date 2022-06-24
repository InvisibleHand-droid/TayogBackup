using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviour
{
    private static NetworkingManager _instance;

    public static NetworkingManager instance { get { return _instance; } }

    public string baseURL = @"http://127.0.0.1:8000/api/player/";

    public static LogIn logIn;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegisterButton()
    {
        Register tempRegister = new Register() { name = nameInputField.text, email = emailInputField.text, password = passwordInputField.text, c_password = confirmPasswordInputField.text };
        StartCoroutine(Register(tempRegister));
    }

    public void OnLoginButton()
    {
        StartCoroutine(LogIn());
    }

    public IEnumerator Register(Register register)
    {
        /*
        // reference code for web requests
        var unityWebReq = new UnityWebRequest(baseURL + "register", "POST");
        string jsonData = JsonUtility.ToJson(register);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        unityWebReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        unityWebReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        unityWebReq.SetRequestHeader("Content-Type", "application/json");

        yield return unityWebReq.SendWebRequest();

        if(unityWebReq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: "+ unityWebReq.error);
        } else
        {
            Debug.Log(unityWebReq.downloadHandler.text);
        } */

        // WWForm is slightly dated
        WWWForm form = new WWWForm();
        form.AddField("name", register.name);
        form.AddField("email", register.email);
        form.AddField("password", register.password);
        form.AddField("c_password", register.c_password);

        UnityWebRequest www = UnityWebRequest.Post(baseURL + "register", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Connection Error");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator LogIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputField.text);
        form.AddField("password", passwordInputField.text);

        UnityWebRequest www = UnityWebRequest.Post(baseURL + "login", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // If connection error
            Debug.Log("Connection Error");
        }
        else
        {
            // If successfully connected
            // Parse Json into our class structure located in Requests.cs
            logIn = JsonUtility.FromJson<LogIn>(www.downloadHandler.text);
            // save player details to the Account Manager instance
            AccountManager.instance.LogIn(emailInputField.text, passwordInputField.text, logIn.data.token);
        }
    }
}
