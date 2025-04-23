using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerNetworkedPropertiesUpdator : MonoBehaviour
{
    public bool updateInterpolatedly = true;

    protected PlayerBaseData thisPlayerBaseData;
    protected pThirdPersonMotor pThirdPersonMotor;
    protected PlayerNetworkedProperties playerNetworkedProperties;

    public void Init()
    {
        pThirdPersonMotor = GetComponent<pThirdPersonMotor>();
        playerNetworkedProperties = GetComponent<PlayerNetworkedProperties>();

        if (playerNetworkedProperties)
        {
            if (pThirdPersonMotor)
            {
                RemoveMovementVariableCallBacks();
                AddMovementVariableCallBacks();
            }
        }
    }

    public void SetThisPlayerBaseData(PlayerBaseData data)
    {
        if(playerNetworkedProperties == null)
            playerNetworkedProperties = GetComponent<PlayerNetworkedProperties>();

        thisPlayerBaseData = data;
        RemovePlayerDataCallbacks(thisPlayerBaseData);
        AddPlayerDataCallbacks(thisPlayerBaseData);
    }
    public void AddPlayerDataCallbacks(PlayerBaseData data)
    {
        data.OnNameChanged += OnPlayerNameChange;
    }

    public void RemovePlayerDataCallbacks(PlayerBaseData data)
    {
        data.OnNameChanged -= OnPlayerNameChange;
    }

    private void RemoveMovementVariableCallBacks()
    {
        pThirdPersonMotor.OnStrafeChanged -= OnStrafeChanged;
        pThirdPersonMotor.OnStopMoveChanged -= OnStopMoveChanged;
        pThirdPersonMotor.OnMoveInputChanged -= OnMoveInputChanged;
        pThirdPersonMotor.OnIsGroundedChanged -= OnIsGroundedChanged;
        pThirdPersonMotor.OnGroundDistanceChanged -= OnGroundDistanceChanged;
        pThirdPersonMotor.OnInputMagnitudeChanged -= OnInputMagnitudeChanged;
        pThirdPersonMotor.OnVerticallAndHorizontalSpeedChanged -= OnVerticallAndHorizontalSpeedChanged;
    }

    private void AddMovementVariableCallBacks()
    {
        pThirdPersonMotor.OnStrafeChanged += OnStrafeChanged;
        pThirdPersonMotor.OnStopMoveChanged += OnStopMoveChanged;
        pThirdPersonMotor.OnMoveInputChanged += OnMoveInputChanged;
        pThirdPersonMotor.OnIsGroundedChanged += OnIsGroundedChanged;
        pThirdPersonMotor.OnGroundDistanceChanged += OnGroundDistanceChanged;
        pThirdPersonMotor.OnInputMagnitudeChanged += OnInputMagnitudeChanged;
        pThirdPersonMotor.OnVerticallAndHorizontalSpeedChanged += OnVerticallAndHorizontalSpeedChanged;
    }
   
    private void OnPlayerNameChange(string value)
    {
        playerNetworkedProperties.SetPlayerName(value);
    }

    private void OnVerticallAndHorizontalSpeedChanged(float vertical, float horizontal)
    {
        playerNetworkedProperties.SetVerticalAndHorizontalSpeed(vertical, horizontal);
    }

    private void OnInputMagnitudeChanged(float value)
    {
        playerNetworkedProperties.SetInputMagnitude(value);
    }

    private void OnGroundDistanceChanged(float value)
    {
        playerNetworkedProperties.SetGroundDistance(value);
    }

    private void OnIsGroundedChanged(bool value)
    {
        playerNetworkedProperties.SetIsGrounded(value);
    }

    private void OnIsSprintingChanged(bool value)
    {
        playerNetworkedProperties.SetIsSprinting(value);
    }

    private void OnMoveInputChanged(Vector3 vector)
    {
        playerNetworkedProperties.SetMoveInput(vector);
    }

    private void OnStopMoveChanged(bool value)
    {
        playerNetworkedProperties.SetStopMove(value);
    }

    private void OnStrafeChanged(bool value)
    {
        playerNetworkedProperties.SetStrafe(value);
    }

    public void AssignMovementNetworkedPropertiesToLocal()
    {
        if (playerNetworkedProperties)
        {
            if (pThirdPersonMotor)
            {
                pThirdPersonMotor.SetStrafe(playerNetworkedProperties.networkStrafe);
                pThirdPersonMotor.SetIsGrounded(playerNetworkedProperties.networkIsGrounded);
                pThirdPersonMotor.SetStopMove(playerNetworkedProperties.networkStopMove);
                pThirdPersonMotor.SetIsSprinting(playerNetworkedProperties.networkIsSprinting);

                if (updateInterpolatedly)
                {
                    pThirdPersonMotor.SetVerticalSpeed(playerNetworkedProperties.networkInterpolatedVerticalSpeed);
                    pThirdPersonMotor.SetHorizontalSpeed(playerNetworkedProperties.networkInterpolatedHorizontalSpeed);
                    pThirdPersonMotor.SetGroundDistance(playerNetworkedProperties.networkInterpolatedGroundDistance);
                    pThirdPersonMotor.SetInputMagnitude(playerNetworkedProperties.networkInterpolatedInputMagnitude);
                    pThirdPersonMotor.SetInput(playerNetworkedProperties.networkInterpolatedMoveInput);
                }
                else
                {
                    pThirdPersonMotor.SetVerticalSpeed(playerNetworkedProperties.networkVerticalSpeed);
                    pThirdPersonMotor.SetHorizontalSpeed(playerNetworkedProperties.networkHorizontalSpeed);
                    pThirdPersonMotor.SetGroundDistance(playerNetworkedProperties.networkGroundDistance);
                    pThirdPersonMotor.SetInputMagnitude(playerNetworkedProperties.networkInputMagnitude);
                    pThirdPersonMotor.SetInput(playerNetworkedProperties.networkMoveInput);
                }
            }
        }
    }

    public void AssignNetworkedPlayerBaseDataToLocal()
    {
        if (playerNetworkedProperties && thisPlayerBaseData != null)
        {
            thisPlayerBaseData.SetName(playerNetworkedProperties.networkPlayerName.Value);
        }
    }

}
