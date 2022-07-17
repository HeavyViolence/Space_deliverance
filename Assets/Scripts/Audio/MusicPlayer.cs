using System;
using System.Collections;
using UnityEngine;

public sealed class MusicPlayer : GlobalInstance<MusicPlayer>, ISavable
{
    public const float MaxPlaybackInterval = 300f;
    public const float MinPlaybackInterval = 5f;
    public const float DefaultPlaybackInterval = 10f;

    [SerializeField] private AudioCollection _music;

    [Range(MinPlaybackInterval, MaxPlaybackInterval)]
    [SerializeField] private float _playbackInterval = DefaultPlaybackInterval;

    private Coroutine _musicPlayer = null;
    private AudioAccess _currentTrackAccess = null;

    public float PlaybackInterval => _playbackInterval;

    public bool IsPlaying => _musicPlayer != null;

    private IEnumerator PlayMusicForever()
    {
        while (true)
        {
            _currentTrackAccess = _music.PlayUnrepeatedRandomAudioClip(Vector2.zero);

            yield return new WaitForSeconds(PlaybackInterval + _currentTrackAccess.Duration);
        }
    }

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        if (!IsPlaying)
        {
            _musicPlayer = StartCoroutine(PlayMusicForever());
        }
    }

    public void Stop()
    {
        if (IsPlaying)
        {
            StopCoroutine(_musicPlayer);
            _musicPlayer = null;
            AudioPlayer.Instance.InterruptPlay(_currentTrackAccess.ID);
        }
    }

    public void SetPlaybackInterval(float value) => _playbackInterval = Mathf.Clamp(value, MinPlaybackInterval, MaxPlaybackInterval);

    public void SetDefaultPlaybackInterval() => SetPlaybackInterval(DefaultPlaybackInterval);

    public object CaptureState() => PlaybackInterval;

    public void RestoreState(object state)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state), "Passed state is null!");
        }

        if (state is float value)
        {
            _playbackInterval = value;
        }
        else
        {
            throw new ArgumentException($"Passed state must be of a {typeof(float)} type!", nameof(state));
        }
    }
}
