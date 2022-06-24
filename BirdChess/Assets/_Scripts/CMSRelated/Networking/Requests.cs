using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requests
{

}

public class Register
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string c_password { get; set; }
}

public class LogIn
{
    public string success;
    public LogInData data;
    public string message;
}

[System.Serializable]
public class LogInData
{
    public string token;
    public string name;
}
public class CmsCosmetic
{
    public string name;
    public string path;
}

[System.Serializable]
public class CmsItem
{
    public string name;
    public string item_description;
    public int amount;
}

[System.Serializable]
public class PlayerItemInventory
{
    public CmsItem[] itemList;
}

public class CmsCharacter
{
    public string name;
    public string path;
    public int exp;
    public int level;
    public int energy;
}

public class CmsPlayerProfile
{
    public string name;
    public int mmr;
    public int energy;
    public bool daily_reward_claimed;
    public int essence;
    public int gems;
    public int quest_id;
    public int daily_login_id;
}

[System.Serializable]
public class ShopItem
{
    public string name;
    public string item_description;
    public float cost;
}
[System.Serializable]
public class ShopItemList
{
    public ShopItem[] itemList;
}