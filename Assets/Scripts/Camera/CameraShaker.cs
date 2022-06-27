using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class CameraShaker : GlobalInstance<CameraShaker>
{
    private const float MaxAmplitude = 1f;
    private const float MaxAttenuation = 2f;
    private const float MaxFrequency = 10f;

    private Rigidbody2D _body;
    private int _activeShakers = 0;
    private bool _shakingEnabled = true;

    public bool ShakingEnabled => _shakingEnabled;

    protected override void Awake()
    {
        base.Awake();

        ConfigureCameraRigidbody(_body = GetComponent<Rigidbody2D>());
    }

    private void ConfigureCameraRigidbody(Rigidbody2D body)
    {
        body.bodyType = RigidbodyType2D.Kinematic;
        body.simulated = true;
        body.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        body.useFullKinematicContacts = false;
        body.sleepMode = RigidbodySleepMode2D.StartAwake;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator ShakeOnce(float amplitude, float attenuation, float frequency, float cutoff)
    {
        _activeShakers++;

        amplitude = Mathf.Clamp(amplitude, 0f, MaxAmplitude);
        attenuation = Mathf.Clamp(attenuation, 0f, MaxAttenuation);
        frequency = Mathf.Clamp(frequency, 0f, MaxFrequency);
        cutoff = Mathf.Clamp01(cutoff);

        float timer = 0f;
        float duration = -1f * Mathf.Log(cutoff, AuxMath.E) / attenuation;

        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;

            float delta = amplitude * Mathf.Exp(-1f * attenuation * timer) * Mathf.Sin(2f * Mathf.PI * frequency * timer);
            float deltaX = delta * AuxMath.RandomSign;
            float deltaY = delta * AuxMath.RandomSign;
            var deltaPos = new Vector2(deltaX, deltaY);

            _body.MovePosition(deltaPos);

            yield return new WaitForFixedUpdate();
        }

        if (--_activeShakers == 0)
        {
            _body.MovePosition(Vector2.zero);
        }
    }

    public void EnableShaking() => _shakingEnabled = true;

    public void DisableShaking() => _shakingEnabled = false;

    public void Shake(float amplitude, float attenuation, float frequency, float cutoff = 0.01f)
    {
        if (_shakingEnabled)
        {
            StartCoroutine(ShakeOnce(amplitude, attenuation, frequency, cutoff));
        }
    }

    public void ShakeOnShotFired() => Shake(0.02f, 2f, 2f);

    public void ShakeOnDeath() => Shake(0.2f, 2f, 1f);

    public void ShakeOnCollision() => Shake(0.1f, 2f, 2f);

    public void ShakeOnPlayerHit() => Shake(0.05f, 2f, 2f);

    public void StopActiveShaking()
    {
        StopAllCoroutines();

        _body.MovePosition(Vector2.zero);
    }
}
