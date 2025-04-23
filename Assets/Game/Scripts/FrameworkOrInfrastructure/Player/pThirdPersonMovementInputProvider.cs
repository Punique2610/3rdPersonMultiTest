using Fusion;
using Fusion.Sockets;
using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pThirdPersonMovementInputProvider : vThirdPersonInput, INetworkRunnerCallbacks
{
    [HideInInspector] public pThirdPersonCamera pTpCamera;

    public System.Action<pMovementInput> OnInputSet;

    protected pMovementInput pMovementInput;

    private NetworkObject _thisPlayerNetworkObject;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        NetworkRoomManager.instance.Runner.AddCallbacks(this);
    }

    private void OnDisable()
    {
        NetworkRoomManager.instance.Runner.RemoveCallbacks(this);
    }
    protected override void Start()
    {
    }

    protected override void FixedUpdate()
    {
        
    }

    public override void OnAnimatorMove()
    {
    }

    protected override void Update()
    {
        if (_thisPlayerNetworkObject)
            if (!_thisPlayerNetworkObject.HasStateAuthority)
                return;
        CameraInput();
        GetInput();
    }

    public void Init()
    {
        _thisPlayerNetworkObject = GetComponent<NetworkObject>();
        if(_thisPlayerNetworkObject != null)
            if (_thisPlayerNetworkObject.HasStateAuthority)
                InitializeTpCamera();
    }

    protected override void InitializeTpCamera()
    {
        if (pTpCamera == null)
        {
            pTpCamera = FindObjectOfType<pThirdPersonCamera>();
            if (pTpCamera == null)
                return;
            if (pTpCamera)
            {
                pTpCamera.SetMainTarget(this.transform);
                pTpCamera.Init();
            }
        }
    }

    protected override void CameraInput()
    {
        if (pTpCamera == null)
            return;

        var Y = Input.GetAxis(rotateCameraYInput);
        var X = Input.GetAxis(rotateCameraXInput);

        pTpCamera.RotateCamera(X, Y);
    }

    protected override void StrafeInput()
    {
        //if (Input.GetKeyDown(strafeInput))
            //cc.Strafe();
    }
    protected override void SprintInput()
    {
        
    }

    public void GetInput()
    {
        pMovementInput.movingAxisInput.x = Input.GetAxis(horizontalInput);
        pMovementInput.movingAxisInput.z = Input.GetAxis(verticallInput);

        pMovementInput.rotateCameraAxisInput.x = Input.GetAxis(rotateCameraXInput);
        pMovementInput.rotateCameraAxisInput.y = Input.GetAxis(rotateCameraYInput);

        pMovementInput.SetButton((int)MovementButtonTypes.Jump, Input.GetKey(jumpInput));
        pMovementInput.SetButton((int)MovementButtonTypes.Sprint, Input.GetKey(sprintInput));

        OnInputSet?.Invoke(pMovementInput);
    }

    #region Unused methods
    public override void MoveInput()
    {

    }

    protected override bool JumpConditions()
    {
        return false;
    }

    protected override void JumpInput()
    {

    }
    #endregion

    #region Unused callbacks

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    #endregion
}

public struct pMovementInput
{
    public pBitWiseButtons pMovementButtons;
    public Vector3 movingAxisInput;
    public Vector3 rotateCameraAxisInput;

    public void SetButton(int btnID, bool state)
    {
        pMovementButtons.Set(btnID, state);
    }

}

public enum MovementButtonTypes
{
    Jump,
    Sprint
}

