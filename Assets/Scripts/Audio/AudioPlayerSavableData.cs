using System;

[Serializable]
public sealed class AudioPlayerSavableData
{
    public float MasterVolume { get; }
    public float MusicVolume { get; }
    public float ShootingVolume { get; }
    public float ExplosionsVolume { get; }
    public float InterfaceVolume { get; }
    public float BackgroundVolume { get; }
    public float NotificationsVolume { get; }
    public float InteractionsVolume { get; }

    public AudioPlayerSavableData(float masterVolume,
                                  float musicVolume,
                                  float shootingVolume,
                                  float explosionsVolume,
                                  float interfaceVolume,
                                  float backgroundVolume,
                                  float notificationsVolume,
                                  float interactionsVolume)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        ShootingVolume = shootingVolume;
        ExplosionsVolume = explosionsVolume;
        InterfaceVolume = interfaceVolume;
        BackgroundVolume = backgroundVolume;
        NotificationsVolume = notificationsVolume;
        InteractionsVolume = interactionsVolume;
    }
}