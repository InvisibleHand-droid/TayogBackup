using Photon.Pun;
using UnityEngine;

public class Singleton<T> : MonoBehaviourPunCallbacks where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance.Equals(this))
        {
            _instance = null;
        }
    }

    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this as T;
        }
    }
}

public class SingletonPersistent<T> : MonoBehaviourPunCallbacks where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance.Equals(this))
        {
            _instance = null;
        }
    }


    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
