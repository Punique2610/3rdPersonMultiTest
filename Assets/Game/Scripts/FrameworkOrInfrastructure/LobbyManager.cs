using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [SerializeField] private NetworkRunner runnerPrefab;
    [SerializeField] private NetworkRoomManager networkRoomManagerPrefab;
 
    public NetworkRunner networkRunner;

    public System.Action OnBeforeJoiningLobby;
    public System.Action OnBeforeJoiningARoom;
    public System.Action OnSessionListUpdated;
    public System.Action<bool> OnAfterJoiningLobby;
    public System.Action<bool> OnAfterCreatingARoom;
    public System.Action<bool> OnAfterJoiningARoom;
    
    public List<SessionInfo> sessionInfos;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitNetworkRoomManager();
        networkRunner = Instantiate(runnerPrefab);
        networkRunner.AddCallbacks(NetworkRoomManager.instance);
        networkRunner.ProvideInput = true;
        EnterLobby();
    }

    public void InitNetworkRoomManager()
    {
        var networkRoomManager = Instantiate(networkRoomManagerPrefab);

        networkRoomManager.OnNewPlayerJoined -= GameManager.instance.OnNewPlayerJoined;
        networkRoomManager.OnNewPlayerJoined += GameManager.instance.OnNewPlayerJoined;

        networkRoomManager.OnAPlayerLeft -= GameManager.instance.OnAPlayerLeft;
        networkRoomManager.OnAPlayerLeft += GameManager.instance.OnAPlayerLeft;

        networkRoomManager.OnRoomListUpdated -= OnRoomListUpdated;
        networkRoomManager.OnRoomListUpdated += OnRoomListUpdated;
    }

    public async void ReturnToMenu()
    {
        await networkRunner.Shutdown();
        NetworkRoomManager.instance.OnReturnToMenu();
    }

    public async void EnterLobby()
    {
        UIManager.Instance.OnEnterLobby();

        OnBeforeJoiningLobby?.Invoke();

        var result = await networkRunner.JoinSessionLobby(SessionLobby.Shared);
        
        if (!networkRunner)
            return;
        if (result.Ok)
            OnAfterJoiningLobby?.Invoke(true);
        else
            OnAfterJoiningLobby?.Invoke(false);
    }

    public void OnRoomListUpdated(List<SessionInfo> list)
    {
        sessionInfos = list;
        OnSessionListUpdated?.Invoke();
    }

    public async void JoinRoom(string roomName)
    {
        OnBeforeJoiningARoom?.Invoke();

        if (!networkRunner)
        {
            OnAfterJoiningARoom.Invoke(false);
            return;
        }

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            SessionName = roomName,
            GameMode = GameMode.Shared
        });

        if (result.Ok)
        {
            networkRunner.AddGlobal(NetworkRoomManager.instance);
            OnAfterJoiningARoom?.Invoke(true);
        }
        else
            OnAfterJoiningARoom?.Invoke(false);
    }
    public async void CreateNewRandomRoom()
    {
        if (networkRunner)
        {
            networkRunner.RemoveCallbacks(NetworkRoomManager.instance);
            await networkRunner.Shutdown();
        }

        networkRunner = Instantiate(runnerPrefab);
        networkRunner.AddCallbacks(NetworkRoomManager.instance);
        networkRunner.ProvideInput = true;

        int sceneBuildIndex = pSceneManager.instance.GetSceneBuildIndexWithDefaultPath("RoomScene.Unity");
        if (sceneBuildIndex < 0) return;

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            SessionName = "Room " + MinorConvenientOperations.GenerateRandomString(12),
            Scene = SceneRef.FromIndex(sceneBuildIndex),
            GameMode = GameMode.Shared
        });

        if (result.Ok)
        {
            networkRunner.AddGlobal(NetworkRoomManager.instance);
            OnAfterCreatingARoom?.Invoke(true);
        }
        else
            OnAfterCreatingARoom?.Invoke(false);
    }

}
