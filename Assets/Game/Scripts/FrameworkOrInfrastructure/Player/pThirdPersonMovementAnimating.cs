using Fusion;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class pThirdPersonMovementAnimating : NetworkBehaviour
{
    protected pThirdPersonAnimator pThirdPersonAnimator;
    protected pThirdPersonController pThirdPersonController;
    protected PlayerNetworkedPropertiesUpdator playerNetworkedPropertiesUpdator;

    private void OnDisable()
    {
        if (pThirdPersonController)
            pThirdPersonController.OnJumpInputSucceeded -= TriggerJumpAnimation;
    }

    public void Init()
    {
        pThirdPersonAnimator = GetComponent<pThirdPersonAnimator>();
        pThirdPersonController = GetComponent<pThirdPersonController>();
        playerNetworkedPropertiesUpdator = GetComponent<PlayerNetworkedPropertiesUpdator>(); 

        if (pThirdPersonController)
        {
            pThirdPersonController.OnJumpInputSucceeded -= TriggerJumpAnimation;
            pThirdPersonController.OnJumpInputSucceeded += TriggerJumpAnimation;
        }
    }

    public override void Spawned()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
    }

    public override void Render()
    {
        if(!HasStateAuthority)
            if (playerNetworkedPropertiesUpdator)
                playerNetworkedPropertiesUpdator.AssignMovementNetworkedPropertiesToLocal();
        if (pThirdPersonAnimator)
            pThirdPersonAnimator.UpdateAnimator();
    }

    public void TriggerJumpAnimation()
    {
        if (!pThirdPersonAnimator) return;
        
        //trigger jump animations
        if (pThirdPersonAnimator.input.sqrMagnitude < 0.1f)
            pThirdPersonAnimator.animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            pThirdPersonAnimator.animator.CrossFadeInFixedTime("JumpMove", .2f);
    }
}
