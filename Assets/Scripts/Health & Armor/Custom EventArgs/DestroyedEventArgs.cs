using System;
using UnityEngine;

public sealed class DestroyedEventArgs : EventArgs
{
    public Vector2 DeathPosition { get; }
    public float Lifespan { get; }

    public DestroyedEventArgs(Vector2 deathPosition, float lifespan)
    {
        DeathPosition = deathPosition;
        Lifespan = lifespan;
    }
}
