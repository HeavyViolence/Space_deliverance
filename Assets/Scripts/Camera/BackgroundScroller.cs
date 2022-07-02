using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public sealed class BackgroundScroller : GlobalInstance<BackgroundScroller>
{
    private const float MinScrollSpeed = 0.001f;
    private const float MaxScrollSpeed = 0.01f;
    private const float DefaultScrollSpeed = 0.002f;

    private const float MinTransitionDuration = 0.5f;
    private const float MaxTransitionDuration = 5f;
    private const float DefaultTransitionDuration = 1f;

    [Range(MinScrollSpeed, MaxScrollSpeed)]
    [SerializeField] private float _scrollSpeed = DefaultScrollSpeed;

    [SerializeField] private List<Material> _backgroundMaterials;
    [SerializeField] private Material _mainMenuMaterial;

    private MeshRenderer _backgroundMeshRenderer;

    private void OnEnable()
    {
        StartCoroutine(AwaitAndSubscribe());
    }

    private void OnDisable()
    {
        SceneLoader.Instance.MainMenuLoaded -= MainMenuLoadedEventHandler;
        SceneLoader.Instance.LevelLoaded -= LevelLoadedEventHandler;
    }

    protected override void Awake()
    {
        base.Awake();

        _backgroundMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        SetRandomLevelBackground();
    }

    private void Start()
    {
        StartCoroutine(ScrollBackgroundForever());
    }

    private IEnumerator AwaitAndSubscribe()
    {
        yield return SceneLoader.Instance != null;

        SceneLoader.Instance.MainMenuLoaded += MainMenuLoadedEventHandler;
        SceneLoader.Instance.LevelLoaded += LevelLoadedEventHandler;

    }

    private void LevelLoadedEventHandler(object sender, System.EventArgs e)
    {
        SetRandomLevelBackground();
    }

    private void MainMenuLoadedEventHandler(object sender, System.EventArgs e)
    {
        SetMainMenuBackground();
    }

    private IEnumerator ScrollBackgroundForever()
    {
        while (true)
        {
            _backgroundMeshRenderer.sharedMaterial.mainTextureOffset += new Vector2(0f, _scrollSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator SmoothlyChangeScrollSpeed(float newValue, float duration)
    {
        float oldValue = _scrollSpeed;
        float transitionDuration = Mathf.Clamp(duration, MinTransitionDuration, MaxTransitionDuration);
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            _scrollSpeed = Mathf.Lerp(oldValue, newValue, timer / transitionDuration);

            yield return null;
        }

        _scrollSpeed = newValue;
    }

    private void SetMainMenuBackground()
    {
        _backgroundMeshRenderer.sharedMaterial = _mainMenuMaterial;
        _backgroundMeshRenderer.sharedMaterial.mainTextureOffset = new Vector2(0f, Random.Range(0f, 1f));
    }

    public void SetNewScrollSpeed(float value, float duration = DefaultTransitionDuration) => StartCoroutine(SmoothlyChangeScrollSpeed(value, duration));

    public void SetScrollSpeedToDefault() => SetNewScrollSpeed(DefaultScrollSpeed, DefaultTransitionDuration);

    public void SetLevelBackground(int index)
    {
        int clampedIndex = Mathf.Clamp(index, 0, _backgroundMaterials.Count - 1);
        _backgroundMeshRenderer.sharedMaterial = _backgroundMaterials[clampedIndex];
        _backgroundMeshRenderer.sharedMaterial.mainTextureOffset = new Vector2(0f, Random.Range(0f, 1f));
    }

    public void SetRandomLevelBackground() => SetLevelBackground(Random.Range(0, _backgroundMaterials.Count));
}
