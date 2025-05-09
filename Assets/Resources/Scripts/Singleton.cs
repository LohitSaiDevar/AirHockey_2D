using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                    {
                        Debug.LogError($"[Singleton] There are multiple instances of singleton {typeof(T)}!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singletonObj = new GameObject($"{typeof(T)} (Singleton)");
                        _instance = singletonObj.AddComponent<T>();
                        DontDestroyOnLoad(singletonObj);

                        Debug.Log($"[Singleton] Created instance of {typeof(T)}.");
                    }
                    else
                    {
                        DontDestroyOnLoad(_instance.gameObject);
                        Debug.Log($"[Singleton] Using existing instance of {typeof(T)}.");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}
