using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using System;

public class pThirdPersonMovementInputFusionAdapter: NetworkBehaviour
{
    protected pThirdPersonController pThirdPersonController;
    protected pThirdPersonMovementInputProvider thirdPersonMovementInputProvider;

    protected pMovementInput currentPMovementInputs;
    protected pBitWiseButtons previousButtons { get; set; }

    public void Init()
    {
        pThirdPersonController = GetComponent<pThirdPersonController>();
        thirdPersonMovementInputProvider = GetComponent<pThirdPersonMovementInputProvider>();

        if(thirdPersonMovementInputProvider)
        {
            thirdPersonMovementInputProvider.OnInputSet -= OnInputSet;
            thirdPersonMovementInputProvider.OnInputSet += OnInputSet;
        }

    }

    private void OnInputSet(pMovementInput input)
    {
        currentPMovementInputs = input;
    }

    public override void Spawned()
    {
        //_pThirdPersonController = GetComponent<pThirdPersonController>();
        //_pThirdPersonMovementInput = GetComponent<pThirdPersonMovementInputProvider>();
    }


    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || !thirdPersonMovementInputProvider)
            return;

        var pressed = currentPMovementInputs.pMovementButtons.GetPressed(previousButtons);
        var released = currentPMovementInputs.pMovementButtons.GetReleased(previousButtons);

        if (pThirdPersonController)
        {
            if (!previousButtons.IsButtonSetDown((int)MovementButtonTypes.Jump))
            {
                if (pressed.IsButtonSetDown((int)MovementButtonTypes.Jump))
                    pThirdPersonController.JumpInput();
            }

            if (!previousButtons.IsButtonSetDown((int)MovementButtonTypes.Sprint))
            {
                if (pressed.IsButtonSetDown((int)MovementButtonTypes.Sprint))
                    pThirdPersonController.SprintInput(true);
            }
            else
            {
                if (released.IsButtonSetDown((int)MovementButtonTypes.Sprint))
                    pThirdPersonController.SprintInput(false);
            }

            pThirdPersonController.MoveInput(currentPMovementInputs.movingAxisInput);
        }

        previousButtons = currentPMovementInputs.pMovementButtons;

    }


}
