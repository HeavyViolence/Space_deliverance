using System;

public sealed class AudioAccess : IEquatable<AudioAccess>
{
    public Guid ID { get; }
    public float Duration { get; }

    public AudioAccess(Guid id, float duration)
    {
        ID = id;
        Duration = duration;
    }

    public bool Equals(AudioAccess other)
    {
        if (other == null) return false;

        return ID.Equals(other.ID);
    }

    public override int GetHashCode() => ID.GetHashCode() ^ Duration.GetHashCode();

    public override string ToString() => $"ID: {ID}; Duration: {Duration}";
}
