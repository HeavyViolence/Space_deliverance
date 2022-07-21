using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public sealed class PlayerMovement : Movement
{
    private PlayerControls _playerControls;

    private Vector2 Velocity => MovementDirection *
                                Time.fixedDeltaTime *
                                new Vector2(Config.HorizontalSpeed.RandomValue, Config.VerticalSpeed.RandomValue);

    private Vector2 MovementDirection
    {
        get
        {
            Vector2 rawMovementDirection = _playerControls.Player.Movement.ReadValue<Vector2>();

            float x = rawMovementDirection.x;
            float y = rawMovementDirection.y;

            if (Body.position.x < Config.LeftBound)
            {
                x = Mathf.Clamp(x, 0f, 1f);
            }

            if (Body.position.x > Config.RightBound)
            {
                x = Mathf.Clamp(x, -1f, 0f);
            }

            if (Body.position.y < Config.LowerBound)
            {
                y = Mathf.Clamp(y, 0f, 1f);
            }

            if (Body.position.y > Config.UpperBound)
            {
                y = Mathf.Clamp(y, -1f, 0f);
            }

            return new Vector2(x, y);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _playerControls = new();
        MovementBehaviour = delegate { Body.MovePosition(Body.position + Velocity); };
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _playerControls.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _playerControls.Disable();
    }
}
