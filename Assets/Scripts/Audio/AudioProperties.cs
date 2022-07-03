using UnityEngine;
using UnityEngine.Audio;

public sealed class AudioProperties
{
    public AudioClip Clip { get; }
    public AudioMixerGroup OutputAudioGroup { get; }
    public float Volume1MeterAway { get; }
    public byte Priority { get; }
    public float SpatialBlend { get; }
    public float Pitch { get; }
    public Vector3 PlayPosition { get; }

    public AudioProperties(AudioClip clip,
                           AudioMixerGroup group,
                           byte priority,
                           float spatialBlend,
                           float pitch,
                           Vector3 playPos,
                           float volume1MeterAway)
    {
        Clip = clip;
        OutputAudioGroup = group;
        Priority = priority;
        SpatialBlend = Mathf.Clamp01(spatialBlend);
        Pitch = Mathf.Clamp(pitch, 0f, 2f);
        PlayPosition = playPos;
        Volume1MeterAway = Mathf.Clamp01(volume1MeterAway);
    }
}