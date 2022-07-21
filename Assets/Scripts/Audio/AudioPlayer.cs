using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public sealed class AudioPlayer : GlobalInstance<AudioPlayer>, ISavable
{
    private const int MaxAudioSources = 32;

    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;

    [SerializeField] private AudioMixer _audioMixer;

    private readonly Dictionary<Guid, AudioSource> _activeAudioSources = new(MaxAudioSources);
    private readonly Stack<AudioSource> _availableAudioSources = new(MaxAudioSources);

    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float ShootingVolume { get; private set; }
    public float ExplosionsVolume { get; private set; }
    public float InterfaceVolume { get; private set; }
    public float BackgroundVolume { get; private set; }
    public float NotificationsVolume { get; private set; }
    public float InteractionsVolume { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SetupAudioSourcePool();
    }

    private void SetupAudioSourcePool()
    {
        for (int i = 0; i < MaxAudioSources; i++)
        {
            var poolItem = new GameObject($"Audio Source #{i + 1}").AddComponent<AudioSource>();

            poolItem.gameObject.transform.parent = Instance.transform;
            SetAudioSourceDefaultState(poolItem);
            _availableAudioSources.Push(poolItem);
        }
    }

    private bool DisableActiveAudioSource(Guid id)
    {
        if (_activeAudioSources.TryGetValue(id, out AudioSource source))
        {
            SetAudioSourceDefaultState(source);
            _activeAudioSources.Remove(id);
            _availableAudioSources.Push(source);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetAudioSourceDefaultState(AudioSource source)
    {
        source.Stop();

        source.clip = null;
        source.outputAudioMixerGroup = null;
        source.mute = true;
        source.bypassEffects = true;
        source.bypassListenerEffects = true;
        source.bypassReverbZones = true;
        source.playOnAwake = false;
        source.loop = false;
        source.priority = byte.MaxValue;
        source.volume = 0f;
        source.spatialBlend = 0f;
        source.pitch = AudioCollection.DefaultPitch;
        source.reverbZoneMix = 0f;

        source.transform.position = Vector3.zero;
        source.gameObject.SetActive(false);
    }

    private AudioAccess ConfigureAudioSource(AudioSource source, AudioProperties properties)
    {
        source.clip = properties.Clip;
        source.outputAudioMixerGroup = properties.OutputAudioGroup;
        source.mute = false;
        source.bypassEffects = false;
        source.bypassListenerEffects = false;
        source.bypassReverbZones = false;
        source.loop = false;
        source.priority = (int)AuxMath.Remap(properties.Priority, 0, byte.MaxValue, byte.MaxValue, 0);
        source.volume = properties.Volume1MeterAway;
        source.spatialBlend = properties.SpatialBlend;
        source.pitch = properties.Pitch;

        source.transform.position = properties.PlayPosition;
        source.gameObject.SetActive(true);

        source.Play();

        var id = Guid.NewGuid();
        var access = new AudioAccess(id, properties.Clip.length);

        _activeAudioSources.Add(id, source);
        StartCoroutine(WaitToDisableActiveAudioSource(access));

        return access;
    }

    private IEnumerator WaitToDisableActiveAudioSource(AudioAccess access)
    {
        yield return new WaitForSeconds(access.Duration);

        DisableActiveAudioSource(access.ID);
    }

    private AudioSource FindAvailableAudioSource()
    {
        if (_availableAudioSources.Count > 0)
        {
            return _availableAudioSources.Pop();
        }

        byte priority = 0;
        Guid id = Guid.Empty;
        AudioSource availableSource = null;

        foreach (var activeSource in _activeAudioSources)
        {
            if (activeSource.Value.priority > priority)
            {
                priority = (byte)activeSource.Value.priority;
                id = activeSource.Key;
                availableSource = activeSource.Value;
            }
        }

        _activeAudioSources.Remove(id);
        SetAudioSourceDefaultState(availableSource);

        return availableSource;
    }

    private void SetVolume(string name, float volume)
    {
        float clampedVolume = Mathf.Clamp(volume, MinVolume, MaxVolume);
        _audioMixer.SetFloat(name, clampedVolume);

        switch (name)
        {
            case "Master Volume":
                {
                    MasterVolume = volume;
                    break;
                }
            case "Shooting Volume":
                {
                    ShootingVolume = volume;
                    break;
                }
            case "Explosions Volume":
                {
                    ExplosionsVolume = volume;
                    break;
                }
            case "Background Volume":
                {
                    BackgroundVolume = volume;
                    break;
                }
            case "Interface Volume":
                {
                    InterfaceVolume = volume;
                    break;
                }
            case "Music Volume":
                {
                    MusicVolume = volume;
                    break;
                }
            case "Interactions Volume":
                {
                    InteractionsVolume = volume;
                    break;
                }
            case "Notifications Volume":
                {
                    NotificationsVolume = volume;
                    break;
                }
        }
    }

    public void SetMasterVolume(float volume) => SetVolume("Master Volume", volume);

    public void SetShootingVolume(float volume) => SetVolume("Shooting Volume", volume);

    public void SetExplosionsVolume(float volume) => SetVolume("Explosions Volume", volume);

    public void SetBackgroundVolume(float volume) => SetVolume("Background Volume", volume);

    public void SetInterfaceVolume(float volume) => SetVolume("Interface Volume", volume);

    public void SetMusicVolume(float volume) => SetVolume("Music Volume", volume);

    public void SetInteractionsVolume(float volume) => SetVolume("Interactions Volume", volume);

    public void SetNotificationsVolume(float volume) => SetVolume("Notifications Volume", volume);

    public AudioAccess Play(AudioProperties properties) => ConfigureAudioSource(FindAvailableAudioSource(), properties);

    public bool InterruptPlay(Guid id) => DisableActiveAudioSource(id);

    public object CaptureState() => new AudioPlayerSavableData(MasterVolume,
                                                               MusicVolume,
                                                               ShootingVolume,
                                                               ExplosionsVolume,
                                                               InterfaceVolume,
                                                               BackgroundVolume,
                                                               NotificationsVolume,
                                                               InteractionsVolume);

    public void RestoreState(object state)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state), "Passed state is null!");
        }

        if (state is AudioPlayerSavableData value)
        {
            SetMasterVolume(value.MasterVolume);
            SetMusicVolume(value.MusicVolume);
            SetShootingVolume(value.ShootingVolume);
            SetExplosionsVolume(value.ExplosionsVolume);
            SetInterfaceVolume(value.InterfaceVolume);
            SetBackgroundVolume(value.BackgroundVolume);
            SetNotificationsVolume(value.NotificationsVolume);
            SetInteractionsVolume(value.InteractionsVolume);
        }
        else
        {
            throw new ArgumentException($"Passed state must be of a {typeof(AudioPlayerSavableData)} type!", nameof(state));
        }
    }
}