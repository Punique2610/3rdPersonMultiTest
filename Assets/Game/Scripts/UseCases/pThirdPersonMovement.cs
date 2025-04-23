using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pThirdPersonMovement : pThirdPersonMovementHandler
{

    public System.Action OnAfterJump;
    protected bool JumpConditions()
    {
        return isGrounded && GroundAngle() < slopeLimit && !isJumping && !stopMove;
    }

    public override void Jump()
    {
        if (!JumpConditions())
            return;

        // trigger jump behaviour
        jumpCounter = jumpTimer;
        isJumping = true;

        OnAfterJump?.Invoke();
    }

}
