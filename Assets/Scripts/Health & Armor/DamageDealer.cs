using System;
using UnityEngine;

public sealed class DamageDealer : MonoBehaviour
{
    public event EventHandler<HitEventArgs> Hit;

    private void Start()
    {
        DisableItselfIfUnused();
    }

    private void OnDisable()
    {
        Hit = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        AttemptToHit(collider);
    }

    private void DisableItselfIfUnused()
    {
        if (Hit == null)
        {
            enabled = false;
        }
    }

    private void AttemptToHit(Collider2D collider)
    {
        if (collider.transform.root.gameObject.TryGetComponent(out IDamageable damageReceiver))
        {
            Hit?.Invoke(this, new HitEventArgs(transform.root.position, damageReceiver));
        }
    }
}
