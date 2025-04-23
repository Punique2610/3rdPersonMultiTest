using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraFusionAdapter : SimulationBehaviour
{
    [SerializeField] private pThirdPersonCamera pThirdPersonCamera;

    public override void Render()
    {
        if (pThirdPersonCamera)
            pThirdPersonCamera.FixedUpdateNetworkTick();
    }
}
