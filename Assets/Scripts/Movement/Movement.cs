using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageDealer))]
public abstract class Movement : MonoBehaviour
{
    [SerializeField] private MovementConfig _movementConfig;

    private DamageDealer _collisionDamageDealer;

    protected Rigidbody2D Body { get; private set; }
    protected Action MovementBehaviour { get; set; }
    protected MovementConfig Config => _movementConfig;

    protected virtual void Awake()
    {
        Body = gameObject.GetComponent<Rigidbody2D>();
        _collisionDamageDealer = gameObject.GetComponent<DamageDealer>();
        SetupRigidbody2D();
    }

    private void OnEnable()
    {
        if (Config.CollisionDamageEnabled)
        {
            _collisionDamageDealer.Hit += CollisionHitEventHandler;
        }
    }

    private void OnDisable()
    {
        if (Config.CollisionDamageEnabled)
        {
            _collisionDamageDealer.Hit -= CollisionHitEventHandler;
        }
    }

    private void FixedUpdate()
    {
        MovementBehaviour?.Invoke();
    }

    private void SetupRigidbody2D()
    {
        Body.bodyType = RigidbodyType2D.Kinematic;
        Body.simulated = true;
        Body.useFullKinematicContacts = true;
        Body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Body.sleepMode = RigidbodySleepMode2D.StartAwake;
        Body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    protected virtual void CollisionHitEventHandler(object sender, HitEventArgs e)
    {
        e.DamageReceiver.ApplyDamage(Config.CollisionDamage.RandomValue);
        Config.CollisionAudio.PlayRandomAudioClip(e.HitPosition);
    }
}
