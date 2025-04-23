using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pThirdPersonAnimator : pThirdPersonMotor
{
    #region Variables                

    public const float walkSpeed = 0.5f;
    public const float runningSpeed = 1f;
    public const float sprintSpeed = 1.5f;

    #endregion

    public virtual void UpdateAnimator()
    {
        if (animator == null || !animator.enabled) return;

        GetDeltaTime();

        animator.SetBool(vAnimatorParameters.IsStrafing, isStrafing);
        animator.SetBool(vAnimatorParameters.IsSprinting, isSprinting);
        animator.SetBool(vAnimatorParameters.IsGrounded, isGrounded);
        animator.SetFloat(vAnimatorParameters.GroundDistance, groundDistance);

        if (isStrafing)
        {
            animator.SetFloat(vAnimatorParameters.InputHorizontal, stopMove ? 0 : horizontalSpeed, strafeSpeed.animationSmooth, deltaTime);
            animator.SetFloat(vAnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, strafeSpeed.animationSmooth, deltaTime);
        }
        else
        {
            animator.SetFloat(vAnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, freeSpeed.animationSmooth, deltaTime);
        }

        animator.SetFloat(vAnimatorParameters.InputMagnitude, stopMove ? 0f : inputMagnitude, isStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, deltaTime);
    }

    public virtual void SetAnimatorMoveSpeed(vMovementSpeed speed)
    {
        Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);

        verticalSpeed = relativeInput.z;
        horizontalSpeed = relativeInput.x;

        var newInput = new Vector2(verticalSpeed, horizontalSpeed);

        if (speed.walkByDefault)
            inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? runningSpeed : walkSpeed);
        else
            inputMagnitude = Mathf.Clamp(isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0, isSprinting ? sprintSpeed : runningSpeed);
        
        OnVerticallAndHorizontalSpeedChanged?.Invoke(verticalSpeed, horizontalSpeed);
        OnInputMagnitudeChanged?.Invoke(inputMagnitude);
    }

    
}
