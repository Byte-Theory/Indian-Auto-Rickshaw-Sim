using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isQuitting = false;

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Try to find existing instance
                    _instance = (T)FindObjectOfType(typeof(T));

                    // If none found, create new GameObject
                    if (_instance == null)
                    {
                        var singletonObj = new GameObject(typeof(T).Name);
                        _instance = singletonObj.AddComponent<T>();
                        DontDestroyOnLoad(singletonObj);
                        Debug.Log($"[Singleton] Created new instance of {typeof(T)}");
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using existing instance of {typeof(T)}");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        // Handle duplicate instances
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} found. Destroying new one.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}