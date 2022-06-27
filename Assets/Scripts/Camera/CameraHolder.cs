using UnityEngine;

public sealed class CameraHolder : GlobalInstance<CameraHolder>
{
    [SerializeField] private Camera _masterCamera;
    [SerializeField] private AudioListener _masterAudioListener;

    public Camera MasterCamera => _masterCamera;
    public AudioListener MasterAudioListener => _masterAudioListener;

    public float ScreenLerftBound { get; private set; }
    public float ScreenRightBound { get; private set; }
    public float ScreenLowerBound { get; private set; }
    public float ScreenUpperBound { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ScreenLerftBound = MasterCamera.ViewportToWorldPoint(Vector3.zero).x;
        ScreenRightBound = MasterCamera.ViewportToWorldPoint(Vector3.right).x;
        ScreenLowerBound = MasterCamera.ViewportToWorldPoint(Vector3.zero).y;
        ScreenUpperBound = MasterCamera.ViewportToWorldPoint(Vector3.up).y;

    }
}
