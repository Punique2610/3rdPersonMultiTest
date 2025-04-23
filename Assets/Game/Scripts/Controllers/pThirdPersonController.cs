using Fusion;
using Invector.vCharacterController;
using UnityEngine;

public class pThirdPersonController : NetworkBehaviour
{

    [HideInInspector] public Camera cameraMain;
    [HideInInspector] public pThirdPersonMovement pThirdPersonMovement;

    public System.Action OnJumpInput;
    public System.Action OnJumpInputSucceeded;
    public System.Action<Vector3> OnMoveInput;


    private void OnDisable()
    {
        if(pThirdPersonMovement)
            pThirdPersonMovement.OnAfterJump -= OnJumpSucceeded;
    }
    private void Update()
    {
        if (!HasStateAuthority)
            return;
        
        UpdateLookDirection();
    }

    public virtual void Init()
    {
        InitilizeMovement();
    }

    public virtual void FixUpdateTick()
    {
        if (!pThirdPersonMovement) return;

        pThirdPersonMovement.UpdateMotor();               // updates the ThirdPersonMotor methods
        pThirdPersonMovement.ControlLocomotionType();     // handle the controller locomotion type and movespeed
        pThirdPersonMovement.ControlRotationType();       // handle the controller rotation type
    }




    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;
        FixUpdateTick();
    }

    protected virtual void InitilizeMovement()
    {
        pThirdPersonMovement = GetComponent<pThirdPersonMovement>();
        if(pThirdPersonMovement)
        {
            pThirdPersonMovement.OnAfterJump -= OnJumpSucceeded;
            pThirdPersonMovement.OnAfterJump += OnJumpSucceeded;
        }
    }

    protected virtual void UpdateLookDirection()
    {        
        if (!cameraMain)
        {
            if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
            else
            {
                cameraMain = Camera.main;
                if(pThirdPersonMovement)
                    pThirdPersonMovement.rotateTarget = cameraMain.transform;
            }
        }

        if (cameraMain && pThirdPersonMovement)
            pThirdPersonMovement.UpdateMoveDirection(cameraMain.transform);
    }

    public void SprintInput(bool state)
    {
        pThirdPersonMovement.Sprint(state);
    }
    public void JumpInput()
    {
        pThirdPersonMovement.Jump();
        OnJumpInput?.Invoke();
    }

    public void OnJumpSucceeded()
    {
        OnJumpInputSucceeded?.Invoke();
    }


    public void MoveInput(Vector3 moveInput)
    {
        pThirdPersonMovement.input.x = moveInput.x;
        pThirdPersonMovement.input.z = moveInput.z;
        pThirdPersonMovement.OnMoveInputChanged?.Invoke(pThirdPersonMovement.input);

        OnMoveInput?.Invoke(moveInput);
    }





}
