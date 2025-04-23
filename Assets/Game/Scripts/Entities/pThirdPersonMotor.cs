using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Fusion;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System;

public class pThirdPersonMotor : vThirdPersonMotor
{

    protected float deltaTime = 0.167f;

    public System.Action<bool> OnStrafeChanged;
    public System.Action<bool> OnStopMoveChanged;
    public System.Action<bool> OnIsGroundedChanged;
    public System.Action<float> OnGroundDistanceChanged;
    public System.Action<float> OnInputMagnitudeChanged;
    public System.Action<float, float> OnVerticallAndHorizontalSpeedChanged;

    public System.Action<Vector3> OnMoveInputChanged;

    public System.Func<float> OnGetRunnerDeltaTime;

    public void GetDeltaTime()
    {
        if (OnGetRunnerDeltaTime != null)
            deltaTime = OnGetRunnerDeltaTime.Invoke();
        else deltaTime = Time.deltaTime;
    }

    #region Set local variables

    public void SetInput(Vector3 value)
    {
        input = value;
    }

    public void SetStrafe(bool value)
    {
        isStrafing = value;
    }

    public void SetStopMove(bool value)
    {
        stopMove = value;
    }

    public void SetIsSprinting(bool networkIsSprinting)
    {
        isSprinting = networkIsSprinting;
    }

    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
    }

    public void SetVerticalSpeed(float value)
    {
        verticalSpeed = value;
    }

    public void SetHorizontalSpeed(float value)
    {
        horizontalSpeed = value;
    }

    public void SetGroundDistance(float value)
    {
        groundDistance = value;
    }

    public void SetInputMagnitude(float value)
    {
        inputMagnitude = value;
    }
    #endregion

    #region Locomotion
    public override void SetControllerMoveSpeed(vMovementSpeed speed)
    {
        GetDeltaTime();

        if (speed.walkByDefault)
            moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * deltaTime);
    }

    public override void MoveCharacter(Vector3 _direction)
    {
        GetDeltaTime();
        // calculate input smooth
        inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * deltaTime);

        if (!isGrounded || isJumping) return;

        _direction.y = 0;
        _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
        _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
        // limit the input
        if (_direction.magnitude > 1f)
            _direction.Normalize();

        Vector3 targetPosition = (useRootMotion ? animator.rootPosition : _rigidbody.position) + _direction * (stopMove ? 0 : moveSpeed) * deltaTime;
        Vector3 targetVelocity = (targetPosition - transform.position) / deltaTime;

        bool useVerticalVelocity = true;
        if (useVerticalVelocity) targetVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = targetVelocity;
    }

    public override void RotateToDirection(Vector3 direction, float rotationSpeed)
    {
        GetDeltaTime();

        if (!jumpAndRotate && !isGrounded) return;
        direction.y = 0f;
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * deltaTime, .1f);
        Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
        transform.rotation = _newRotation;
    }
    public override void CheckSlopeLimit()
    {
        if (input.sqrMagnitude < 0.1) return;

        RaycastHit hitinfo;
        var hitAngle = 0f;

        if (Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), transform.position + moveDirection.normalized * (_capsuleCollider.radius + 0.2f), out hitinfo, groundLayer))
        {
            hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

            var targetPoint = hitinfo.point + moveDirection.normalized * _capsuleCollider.radius;
            if ((hitAngle > slopeLimit) && Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), targetPoint, out hitinfo, groundLayer))
            {
                hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                if (hitAngle > slopeLimit && hitAngle < 85f)
                {
                    stopMove = true;
                    return;
                }
            }
        }
        stopMove = false;

        OnStopMoveChanged?.Invoke(stopMove);
    }
    #endregion

    #region Jump Methods

    protected override void ControlJumpBehaviour()
    {
        if (!isJumping) return;

        GetDeltaTime();

        jumpCounter -= deltaTime;
        if (jumpCounter <= 0)
        {
            jumpCounter = 0;
            isJumping = false;
        }
        // apply extra force to the jump height   
        var vel = _rigidbody.velocity;
        vel.y = jumpHeight;
        _rigidbody.velocity = vel;
    }

    public override void AirControl()
    {
        if ((isGrounded && !isJumping)) return;

        GetDeltaTime();

        if (transform.position.y > heightReached) heightReached = transform.position.y;

        inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * deltaTime);

        if (jumpWithRigidbodyForce && !isGrounded)
        {
            _rigidbody.AddForce(moveDirection * airSpeed * deltaTime, ForceMode.VelocityChange);
            return;
        }

        moveDirection.y = 0;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

        Vector3 targetPosition = _rigidbody.position + (moveDirection * airSpeed) * deltaTime;
        Vector3 targetVelocity = (targetPosition - transform.position) / deltaTime;

        targetVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, airSmooth * deltaTime);
    }

    #endregion

    #region Ground Check                

    protected override void CheckGroundDistance()
    {
        if (_capsuleCollider != null)
        {
            // radius of the SphereCast
            float radius = _capsuleCollider.radius * 0.9f;
            var dist = 10f;
            // ray for RayCast
            Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
            // raycast for check the ground distance
            if (Physics.Raycast(ray2, out groundHit, (colliderHeight / 2) + dist, groundLayer) && !groundHit.collider.isTrigger)
                dist = transform.position.y - groundHit.point.y;
            // sphere cast around the base of the capsule to check the ground distance
            if (dist >= groundMinDistance)
            {
                Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                Ray ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out groundHit, _capsuleCollider.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
                {
                    Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
                    float newDist = transform.position.y - groundHit.point.y;
                    if (dist > newDist) dist = newDist;
                }
            }

            groundDistance = (float)System.Math.Round(dist, 2);
            OnGroundDistanceChanged?.Invoke(groundDistance);
        }
    }
    protected override void CheckGround()
    {
        GetDeltaTime();

        CheckGroundDistance();
        ControlMaterialPhysics();

        if (groundDistance <= groundMinDistance)
        {
            isGrounded = true;

            if (!isJumping && groundDistance > 0.05f)
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * deltaTime), ForceMode.VelocityChange);

            heightReached = transform.position.y;
        }
        else
        {
            if (groundDistance >= groundMaxDistance)
            {

                // set IsGrounded to false 
                isGrounded = false;

                // check vertical velocity
                verticalVelocity = _rigidbody.velocity.y;
                // apply extra gravity when falling
                if (!isJumping)
                {
                    _rigidbody.AddForce(transform.up * extraGravity * deltaTime, ForceMode.VelocityChange);
                }
            }
            else if (!isJumping)
            {
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * deltaTime), ForceMode.VelocityChange);
            }
        }

        OnIsGroundedChanged?.Invoke(isGrounded);
    }

    

    #endregion


}