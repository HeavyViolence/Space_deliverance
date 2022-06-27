using UnityEngine;

public abstract class SceneInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        SetupInstance();
    }

    private void SetupInstance()
    {
        if (Instance == null)
        {
            Instance = (T)(MonoBehaviour)this;
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }
}
