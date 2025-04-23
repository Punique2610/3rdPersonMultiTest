using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkedProperties : NetworkBehaviour
{
    [HideInInspector][Networked] public NetworkBool networkStrafe { get; protected set; }
    [HideInInspector][Networked] public NetworkBool networkStopMove { get; protected set; }
    [HideInInspector][Networked] public NetworkBool networkIsGrounded { get; protected set; }
    [HideInInspector][Networked] public NetworkBool networkIsSprinting { get; protected set; }

    [HideInInspector][Networked] public float networkVerticalSpeed { get; protected set; }
    [HideInInspector][Networked] public float networkHorizontalSpeed { get; protected set; }
    [HideInInspector][Networked] public float networkGroundDistance { get; protected set; }
    [HideInInspector][Networked] public float networkInputMagnitude { get; protected set; }

    [HideInInspector][Networked] public NetworkString<_32> networkPlayerName { get; protected set; }

    [HideInInspector][Networked] public Vector3 networkMoveInput { get; set; }

    public float networkInterpolatedVerticalSpeed => new NetworkBehaviourBufferInterpolator(this).Float(nameof(networkVerticalSpeed));
    public float networkInterpolatedHorizontalSpeed => new NetworkBehaviourBufferInterpolator(this).Float(nameof(networkHorizontalSpeed));
    public float networkInterpolatedGroundDistance => new NetworkBehaviourBufferInterpolator(this).Float(nameof(networkGroundDistance));
    public float networkInterpolatedInputMagnitude => new NetworkBehaviourBufferInterpolator(this).Float(nameof(networkInputMagnitude));
    public Vector3 networkInterpolatedMoveInput => new NetworkBehaviourBufferInterpolator(this).Vector3(nameof(networkMoveInput));

    public void SetPlayerName(string value)
    {
        if(HasStateAuthority)
            networkPlayerName = value;
    }

    public void SetStrafe(bool value)
    {
        networkStrafe = value;
    }

    public void SetStopMove(bool value)
    {
        networkStopMove = value;
    }

    public void SetIsGrounded(bool value)
    {
        networkIsGrounded = value;
    }

    public void SetIsSprinting(bool value)
    {
        networkIsSprinting = value;
    }

    public void SetVerticalAndHorizontalSpeed(float vertical, float horizontal)
    {
        networkVerticalSpeed = vertical;
        networkHorizontalSpeed = horizontal;
    }

    public void SetGroundDistance(float value)
    {
        networkGroundDistance = value;
    }

    public void SetInputMagnitude(float value)
    {
        networkInputMagnitude = value; 
    }

    public void SetMoveInput(Vector3 value)
    {
        networkMoveInput = value;
    }

    public override void Render()
    {
        
    }
}
