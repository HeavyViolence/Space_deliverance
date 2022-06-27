using UnityEngine;

public abstract class HiddenGlobalInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    protected virtual void Awake()
    {
        SetupInstance();
    }

    private void SetupInstance()
    {
        if (_instance == null)
        {
            _instance = (T)(MonoBehaviour)this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }
}