using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio Collection", menuName = "Configs/Audio Collection")]
public sealed class AudioCollection : ScriptableObject
{
    public const float MinPitch = -3f;
    public const float MaxPitch = 3f;
    public const float DefaultPitch = 1f;

    [SerializeField] private List<AudioClip> _audioClips;

    [SerializeField] private AudioMixerGroup _outputAudioGroup;

    [SerializeField] private float _volume1MeterAway = 1f;
    [SerializeField] private float _volume1MeterAwayRandomness = 0f;

    [SerializeField] private byte _priority = 0;

    [SerializeField] private float _spatialBlend = 0f;
    [SerializeField] private float _spatialBlendRandomness = 0f;

    [SerializeField] private float _pitch = DefaultPitch;
    [SerializeField] private float _pitchRandomness = 0f;

    private readonly HashSet<int> _playedClipsIndexers = new();
    private int _nextAudioClipIndex = 0;

    public AudioMixerGroup OutputAudioGroup => _outputAudioGroup;

    public RangedFloat Volume1MeterAway { get; private set; }

    public byte Priority => _priority;

    public RangedFloat SpatialBlend { get; private set; }

    public RangedFloat Pitch { get; private set; }

    private void OnEnable()
    {
        Volume1MeterAway = new RangedFloat(_volume1MeterAway, _volume1MeterAway * _volume1MeterAwayRandomness, 0f, 1f);
        SpatialBlend = new RangedFloat(_spatialBlend, _spatialBlend * _spatialBlendRandomness, 0f, 1f);
        Pitch = new RangedFloat(_pitch, _pitch * _pitchRandomness, MinPitch, MaxPitch);
    }

    public AudioClip GetRandomAudioClip() => _audioClips[Random.Range(0, _audioClips.Count)];

    public AudioClip GetUnrepeatedRandomAudioClip()
    {
        int index;

        if (_playedClipsIndexers.Count == 0)
        {
            index = Random.Range(0, _audioClips.Count);
        }
        else
        {
            index = AuxMath.GetRandomFromRangeWithExclusions(0, _audioClips.Count, _playedClipsIndexers);

            if (_playedClipsIndexers.Count == _audioClips.Count - 1)
            {
                _playedClipsIndexers.Clear();
            }
        }

        _playedClipsIndexers.Add(index);

        return _audioClips[index];
    }

    private AudioClip GetNextAudioClip() => _audioClips[_nextAudioClipIndex++ % _audioClips.Count];

    private AudioAccess PlayAudioClip(AudioClip clip, Vector2 position)
    {
        AudioProperties properties = new(clip,
                                         _outputAudioGroup,
                                         _priority,
                                         SpatialBlend.RandomValue,
                                         Pitch.RandomValue,
                                         position,
                                         Volume1MeterAway.RandomValue);

        return AudioPlayer.Instance.Play(properties);
    }

    public AudioAccess PlayRandomAudioClip(Vector2 position) => PlayAudioClip(GetRandomAudioClip(), position);

    public AudioAccess PlayNextAudioClip(Vector2 position) => PlayAudioClip(GetNextAudioClip(), position);

    public AudioAccess PlayUnrepeatedRandomAudioClip(Vector2 position) => PlayAudioClip(GetUnrepeatedRandomAudioClip(), position);
}