using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetworkCompositeRoot : NetworkBehaviour
{
    [SerializeField] private pThirdPersonMovement _pThirdPersonMovement;
    [SerializeField] private PlayerNetworkedPropertiesUpdator _playerNetworkedPropertiesUpdator;
    [SerializeField] private pThirdPersonMovementInputProvider _pThirdPersonMovementInputProvider;

    private pThirdPersonController _pThirdPersonController;
    private pThirdPersonMovementAnimating _pThirdPersonMovementAnimating;
    private pThirdPersonMovementInputFusionAdapter _pThirdPersonMovementInputFusionAdapter;

    public System.Action<GameObject> OnPlayerPostNetworkSpawned;

    public IEnumerator Start()
    {
        _pThirdPersonController = GetComponent<pThirdPersonController>();
        _pThirdPersonMovementAnimating = GetComponent<pThirdPersonMovementAnimating>();
        _pThirdPersonMovementInputFusionAdapter = GetComponent<pThirdPersonMovementInputFusionAdapter>();

        yield return new WaitUntil(() => 
        _pThirdPersonMovement && 
        _playerNetworkedPropertiesUpdator &&
        _pThirdPersonMovementInputProvider);
        
        _pThirdPersonMovement.Init();
        _pThirdPersonController.Init();
        _pThirdPersonMovementAnimating.Init();
        _playerNetworkedPropertiesUpdator.Init();
        _pThirdPersonMovementInputProvider.Init();
        _pThirdPersonMovementInputFusionAdapter.Init();

        //yield return new WaitUntil(() => RoomManager.Instance != null);

        //RoomManager.Instance.OnNewPlayer(this.transform);

    }

    public override void Spawned()
    {
        OnPlayerPostNetworkSpawned?.Invoke(gameObject);
    }


}
