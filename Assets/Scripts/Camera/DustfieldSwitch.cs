using System.Collections;
using UnityEngine;

public sealed class DustfieldSwitch : HiddenGlobalInstance<DustfieldSwitch>
{
    [SerializeField] private ParticleSystem _dustfield;

    private void OnEnable()
    {
        _dustfield.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        StartCoroutine(AwaitAndSubscribe());
    }

    private void OnDisable()
    {
        SceneLoader.Instance.LevelLoaded -= LevelLoadedEventHandler;
        SceneLoader.Instance.MainMenuLoaded -= MainMenuLoadedEventHandler;
    }

    private IEnumerator AwaitAndSubscribe()
    {
        yield return SceneLoader.Instance != null;

        SceneLoader.Instance.LevelLoaded += LevelLoadedEventHandler;
        SceneLoader.Instance.MainMenuLoaded += MainMenuLoadedEventHandler;
    }

    private void LevelLoadedEventHandler(object sender, System.EventArgs e)
    {
        _dustfield.Play(true);
    }

    private void MainMenuLoadedEventHandler(object sender, System.EventArgs e)
    {
        _dustfield.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
