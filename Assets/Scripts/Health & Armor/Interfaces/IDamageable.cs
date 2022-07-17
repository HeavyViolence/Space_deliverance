using System;

public interface IDamageable
{
    public event EventHandler<DamageReceivedEventArgs> DamageReceived;
    public Guid ID { get; }
    public void ApplyDamage(float damage);
}
