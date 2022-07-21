using UnityEngine;

[CreateAssetMenu(fileName = "Movement Config", menuName = "Configs/Movement/Basic Movement Config")]
public class MovementConfig : ScriptableObject
{
    public const float MaxSpeed = 20f;

    public const float MinBoundsDisplacementFactor = 0f;
    public const float MaxBoundsDisplacementFactor = 2f;
    public const float DefaultBoundDisplacementFactor = 1f;

    public const float MinCollisionDamage = 100f;
    public const float MaxCollisionDamage = 10000f;
    public const float DefaultCollisionDamage = 1000f;

    [SerializeField] private float _horizontalSpeed = 0f;
    [SerializeField] private float _horizontalSpeedRandomness = 0f;

    [SerializeField] private float _verticalSpeed = 0f;
    [SerializeField] private float _verticalSpeedRandomness = 0f;

    [SerializeField] private bool _customMovementBoundsEnabled = false;
    [SerializeField] private float _upperBoundDisplacementFactor = DefaultBoundDisplacementFactor;
    [SerializeField] private float _lowerBoundDisplacementFactor = DefaultBoundDisplacementFactor;
    [SerializeField] private float _verticalBoundsDisplacementFactor = DefaultBoundDisplacementFactor;

    [SerializeField] private bool _collisionDamageEnabled = false;
    [SerializeField] private float _collisionDamage = DefaultCollisionDamage;
    [SerializeField] private float _collisionDamageRandomness = 0f;
    [SerializeField] private AudioCollection _collisionAudio;
    [SerializeField] private bool _cameraShakeOnCollisionEnabled = false;

    public RangedFloat HorizontalSpeed { get; private set; }
    public RangedFloat VerticalSpeed { get; private set; }
    public float Speed2D
    {
        get
        {
            if (HorizontalSpeed.RandomValue == 0f && VerticalSpeed.RandomValue == 0f) return 0f;

            if (HorizontalSpeed.RandomValue != 0f && VerticalSpeed.RandomValue == 0f) return HorizontalSpeed.RandomValue;

            if (HorizontalSpeed.RandomValue == 0f && VerticalSpeed.RandomValue != 0f) return VerticalSpeed.RandomValue;

            else return Mathf.Sqrt(HorizontalSpeed.RandomValue * HorizontalSpeed.RandomValue +
                                   VerticalSpeed.RandomValue * VerticalSpeed.RandomValue);
        }
    }

    public bool CustomMovementBoundsEnabled => _customMovementBoundsEnabled;
    public float LeftBound => _customMovementBoundsEnabled ? CameraHolder.Instance.ScreenLeftBound * _verticalBoundsDisplacementFactor
                                                           : CameraHolder.Instance.ScreenLeftBound;
    public float RightBound => _customMovementBoundsEnabled ? CameraHolder.Instance.ScreenRightBound * _verticalBoundsDisplacementFactor
                                                            : CameraHolder.Instance.ScreenRightBound;
    public float UpperBound => _customMovementBoundsEnabled ? CameraHolder.Instance.ScreenUpperBound * _upperBoundDisplacementFactor
                                                            : CameraHolder.Instance.ScreenUpperBound;
    public float LowerBound => _customMovementBoundsEnabled ? CameraHolder.Instance.ScreenLowerBound * _lowerBoundDisplacementFactor
                                                            : CameraHolder.Instance.ScreenLowerBound;

    public bool CollisionDamageEnabled => _collisionDamageEnabled;
    public RangedFloat CollisionDamage { get; private set; }
    public AudioCollection CollisionAudio => _collisionAudio;
    public bool CameraShakeOnCollisionEnabled => _cameraShakeOnCollisionEnabled;

    private void OnEnable()
    {
        HorizontalSpeed = new RangedFloat(_horizontalSpeed, _horizontalSpeed * _horizontalSpeedRandomness, 0f, MaxSpeed);
        VerticalSpeed = new RangedFloat(_verticalSpeed, _verticalSpeed * _verticalSpeedRandomness, 0f, MaxSpeed);

        CollisionDamage = _collisionDamageEnabled ? new RangedFloat(_collisionDamage, _collisionDamage * _collisionDamageRandomness, MinCollisionDamage, MaxCollisionDamage)
                                                  : RangedFloat.Zero;
    }
}
