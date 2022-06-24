using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    private static AccountManager _instance;

    public static AccountManager instance { get { return _instance; } }


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

    public static string LoggedIn_Username { get; protected set; } // stores username once logged in
    private static string LoggedIn_Password = ""; // stores password once logged in
    private static string LoggedIn_AccessToken = "Expired Access Token";

    public string baseURL = @"http://127.0.0.1:8000/api/player/";

    public static string LoggedIn_Data { get; protected set; }
    public CmsPlayerProfile MyProfile { get; protected set; }
    public PlayerItemInventory MyItems { get; protected set; }
    public CmsCharacter[] MyCharacters { get; protected set; }
    public CmsCosmetic[] MyCosmetics { get; protected set; }

    public ShopItemList shopItems { get; protected set; }
    public static bool IsLoggedIn { get; protected set; }

    public string loggedInSceneName = "testLoginProfile";
    public string loggedOutSceneName = "testLogin";

    public void LogOut()
    {
        LoggedIn_Username = "";
        LoggedIn_Password = "";
        LoggedIn_AccessToken = "";

        MyItems = null;
        MyCharacters = null;
        MyCosmetics = null;

        IsLoggedIn = false;

        SceneManager.LoadScene(loggedOutSceneName);
    }
    public void LogIn(string username, string password, string token)
    {
        LoggedIn_Username = username;
        LoggedIn_Password = password;
        LoggedIn_AccessToken = token;

        IsLoggedIn = true;

        //SceneManager.LoadScene(loggedInSceneName);
        StartCoroutine(GetPlayerInventoryItems());
    }

    public IEnumerator Profile()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "details");
        www.SetRequestHeader("Authorization", "Bearer " + LoggedIn_AccessToken);
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
            // save player details
            MyProfile = JsonUtility.FromJson<CmsPlayerProfile>(www.downloadHandler.text);
            print("NAME: " + MyProfile.name);
        }
    }

    public IEnumerator GetShopItems()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "shop/items");
        www.SetRequestHeader("Authorization", "Bearer " + LoggedIn_AccessToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // If connection error
            Debug.Log("Connection Error");
        }
        else
        {
            // If successfully connected
            // the normal JsonUtility cannot be used because our shop data contains arrays which arent supported by it
            // shopItems = JsonUtility.FromJson<ShopItemList>(www.downloadHandler.text);
            // so we need to serialize the shop items and shop items list then parse it like this:
            shopItems = JsonUtility.FromJson<ShopItemList>("{\"itemList\":" + www.downloadHandler.text + "}");
            foreach (ShopItem tempRef in shopItems.itemList)
            {
                Debug.Log("Name: " + tempRef.name + " | Description: " + tempRef.item_description);
            }
        }
    }

    public IEnumerator GetPlayerInventoryItems()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "items");
        www.SetRequestHeader("Authorization", "Bearer " + LoggedIn_AccessToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // If connection error
            Debug.Log("Connection Error");
        }
        else
        {
            // If successfully connected
            // the normal JsonUtility cannot be used because our shop data contains arrays which arent supported by it
            // shopItems = JsonUtility.FromJson<ShopItemList>(www.downloadHandler.text);
            // so we need to serialize the shop items and shop items list then parse it like this:
            MyItems = JsonUtility.FromJson<PlayerItemInventory>("{\"itemList\":" + www.downloadHandler.text + "}");
            foreach (CmsItem tempRef in MyItems.itemList)
            {
                Debug.Log("Name: " + tempRef.name + " | Description: " + tempRef.item_description + " | Amount: " + tempRef.amount);
            }
        }
    }
}
