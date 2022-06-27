using UnityEngine;

public abstract class HiddenSceneInstance<T> : MonoBehaviour where T : MonoBehaviour
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
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }
}