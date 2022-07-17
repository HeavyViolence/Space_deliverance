using UnityEngine;
using System;

public interface IMovementBehaviourSwitch
{
    public void SetMovementBehaviour(Action behaviour, out Rigidbody2D body);
}
