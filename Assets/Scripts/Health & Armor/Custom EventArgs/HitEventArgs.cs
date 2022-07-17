using System;
using UnityEngine;

public sealed class HitEventArgs : EventArgs
{
    public Vector2 HitPosition { get; }
    public IDamageable DamageReceiver { get; }

    public HitEventArgs(Vector2 hitPosition, IDamageable damageReceiver)
    {
        HitPosition = hitPosition;
        DamageReceiver = damageReceiver;
    }
}
