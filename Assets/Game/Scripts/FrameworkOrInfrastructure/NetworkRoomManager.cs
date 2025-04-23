using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NetworkRoomManager : SimulationBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRoomManager instance;

    [SerializeField] private GameObject playerPrefab;

    public System.Action OnNewPlayerJoined;
    public System.Action OnAPlayerLeft;
    public System.Action<List<SessionInfo>> OnRoomListUpdated;

    [HideInInspector] public string sceneLoadAfterShutdown = "";
    public void Awake()
    {
        var dups = FindObjectsOfType<NetworkRoomManager>();
        foreach (var dup in dups)
            if (dup != this)
                Destroy(dup.gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public float GetRunnerDeltaTime()
    {
        return Runner.DeltaTime != 0 ? Runner.DeltaTime : Time.deltaTime;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!playerPrefab) return;
        
        if (player == runner.LocalPlayer)
        {
            NetworkObject go = null;
            if (RoomManager.instance.defaultSpawnPoint)
                go = Runner.Spawn(playerPrefab, new Vector3(UnityEngine.Random.Range(RoomManager.instance.defaultSpawnPoint.position.x - 5, RoomManager.instance.defaultSpawnPoint.position.x + 5),
                    1,
                    UnityEngine.Random.Range(RoomManager.instance.defaultSpawnPoint.position.z - 5, RoomManager.instance.defaultSpawnPoint.position.z + 5)),
                    Quaternion.identity);
            else
                go = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

            var cameraNetworkHandler = FindObjectOfType<ThirdPersonCameraFusionAdapter>();
            if (cameraNetworkHandler)
                Runner.AddGlobal(cameraNetworkHandler);

            go.GetComponent<pThirdPersonMotor>().OnGetRunnerDeltaTime -= GetRunnerDeltaTime;
            go.GetComponent<pThirdPersonMotor>().OnGetRunnerDeltaTime += GetRunnerDeltaTime;

            StartCoroutine(WaitForPlayerToFinishSpawning(0.5f));
        }
        else
            StartCoroutine(WaitForPlayerToFinishSpawning(1f));
    }

    public IEnumerator WaitForPlayerToFinishSpawning(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        OnNewPlayerJoined?.Invoke();
    }

    public void OnReturnToLobby()
    {
        sceneLoadAfterShutdown = "LobbyScene";
        Runner.Shutdown();
    }

    public void OnReturnToMenu()
    {
        sceneLoadAfterShutdown = "MainMenuScene";
        if (Runner)
            Runner.Shutdown();
        else
        {
            pSceneManager.instance.LoadNewSceneAsync(sceneLoadAfterShutdown);
            Destroy(gameObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        StartCoroutine(WaitForPlayerToLeft());
    }

    public IEnumerator WaitForPlayerToLeft()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        OnAPlayerLeft?.Invoke();
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        if(sceneLoadAfterShutdown != "")
            pSceneManager.instance.LoadNewSceneAsync(sceneLoadAfterShutdown);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnRoomListUpdated?.Invoke(sessionList);
    }

    #region Unused Callbacks

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }
    #endregion
}
