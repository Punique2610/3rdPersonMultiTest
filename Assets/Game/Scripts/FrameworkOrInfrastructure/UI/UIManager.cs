using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject mainMenuUIPrefab;
    [SerializeField] private RoomUI roomUIPrefab;
    [SerializeField] private LobbyUI lobbyUIPrefab;

    private MainMenuUI mainMenuUI;
    private RoomUI roomUI;
    private LobbyUI lobbyUI;

    private void Awake()
    {
        var dups = FindObjectsOfType<UIManager>();
        foreach (var dup in dups)
            if (dup != this)
                Destroy(dup.gameObject);

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        OnMainMenu();
    }

    public void OnMainMenu()
    {
        if (mainMenuUIPrefab)
        {
            mainMenuUI = Instantiate(mainMenuUIPrefab).GetComponentInChildren<MainMenuUI>();
            mainMenuUI.Init();

            pSceneManager.instance.OnStartLoadingScene -= mainMenuUI.OnStartLoadNewScene;
            pSceneManager.instance.OnStartLoadingScene += mainMenuUI.OnStartLoadNewScene;

            pSceneManager.instance.OnDuringLoadingScene -= mainMenuUI.SetProgressBar;
            pSceneManager.instance.OnDuringLoadingScene += mainMenuUI.SetProgressBar;
        }
    }

    public void OnEnterLobby()
    {
        lobbyUI = Instantiate(lobbyUIPrefab);
        lobbyUI.Init();

        LobbyManager.instance.OnBeforeJoiningLobby -= lobbyUI.OnBeforeJoiningLobby;
        LobbyManager.instance.OnBeforeJoiningLobby += lobbyUI.OnBeforeJoiningLobby;

        LobbyManager.instance.OnAfterJoiningLobby -= lobbyUI.OnAfterJoiningLobby;
        LobbyManager.instance.OnAfterJoiningLobby += lobbyUI.OnAfterJoiningLobby;

        LobbyManager.instance.OnSessionListUpdated -= lobbyUI.UpdateRoomSessionUIs;
        LobbyManager.instance.OnSessionListUpdated += lobbyUI.UpdateRoomSessionUIs;

        LobbyManager.instance.OnBeforeJoiningARoom -= lobbyUI.OnBeforeJoiningARoom;
        LobbyManager.instance.OnBeforeJoiningARoom += lobbyUI.OnBeforeJoiningARoom;

        LobbyManager.instance.OnAfterCreatingARoom -= lobbyUI.SetCreateRoomBtnStateReverse;
        LobbyManager.instance.OnAfterCreatingARoom += lobbyUI.SetCreateRoomBtnStateReverse;
    }

    public void OnEnterRoom()
    {
        roomUI = Instantiate(roomUIPrefab);
        roomUI.Init();

        GameManager.instance.OnNewPlayerSpawned -= roomUI.OnNewPlayerSpawned;
        GameManager.instance.OnNewPlayerSpawned += roomUI.OnNewPlayerSpawned;
    }

}
