using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : GlobalInstance<SceneLoader>
{
    public event EventHandler LevelLoadingStarted;
    public event EventHandler LevelLoaded;

    public event EventHandler MainMenuLoadingStarted;
    public event EventHandler MainMenuLoaded;

    private void OnEnable()
    {
        LoadScene("Test Level");
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName.Equals("Main Menu"))
        {
            MainMenuLoadingStarted?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            LevelLoadingStarted?.Invoke(this, EventArgs.Empty);
        }

        var operation = SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(AwaitForSceneLoadingIsDone(operation, sceneName));
    }

    private IEnumerator AwaitForSceneLoadingIsDone(AsyncOperation op, string sceneName)
    {
        yield return op.isDone;

        if (sceneName.Equals("Main Menu"))
        {
            MainMenuLoaded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            LevelLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}