using System;

public sealed class DamageReceivedEventArgs : EventArgs
{
    public float DamageReceived { get; }
    public float DamageDealt { get; }
    public float DamageEfficiency => DamageDealt / DamageReceived;

    public DamageReceivedEventArgs(float damageReceived, float damageDealt)
    {
        DamageReceived = damageReceived;
        DamageDealt = damageDealt;
    }
}
