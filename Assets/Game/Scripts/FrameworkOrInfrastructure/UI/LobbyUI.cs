using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button returnToMenuBtn;
    [SerializeField] private Button createRandomRoomBtn;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Transform entriesScrollContentParent;
    [SerializeField] private RoomSessionEntryUI roomSessionEntryUIPrefab;

    [SerializeField] private List<RoomSessionEntryUI> roomSessionEntryUIs = new List<RoomSessionEntryUI>();

    public void Init()
    {
        if(returnToMenuBtn)
        {
            returnToMenuBtn.onClick.RemoveAllListeners();
            returnToMenuBtn.onClick.AddListener(OnReturnToMenuBtnClicked);
        }

        if (createRandomRoomBtn)
        {
            createRandomRoomBtn.interactable = false;
            createRandomRoomBtn.onClick.RemoveListener(OnCreateRandomRoomBtnClicked);
            createRandomRoomBtn.onClick.AddListener(OnCreateRandomRoomBtnClicked);
        }

        if (playerNameInputField)
        {
            playerNameInputField.onValueChanged.RemoveListener(OnPlayerNameChanged);
            playerNameInputField.onValueChanged.AddListener(OnPlayerNameChanged);
        }

        playerNameInputField.text = GameManager.instance.thisPlayerServerData.Name;
    }

    public void SetSessionListJoinBtnState(bool state)
    {
        foreach (var item in roomSessionEntryUIs)
            item.SetJoinBtnState(state);
    }
    public void UpdateRoomSessionUIs()
    {
        var list = LobbyManager.instance.sessionInfos;

        foreach (var session in list)
        {
            var found = roomSessionEntryUIs.Find(s => s.RoomName == session.Name);
            if (found)
                UpdateSessionUI(session, found);
            else
                CreateRoomSessionUI(session);
        }

        var removeUIsList = new List<RoomSessionEntryUI>();
        foreach (var ui in roomSessionEntryUIs)
        {
            var found = list.Find(s => s.Name == ui.RoomName);
            if (!found)
                removeUIsList.Add(ui);
        }

        foreach (var ui in removeUIsList)
            roomSessionEntryUIs.Remove(ui);
    }
    public void SetPlayerName(string value)
    {
        playerNameInputField.text = value;
    }
    public void SetCreateRoomBtnState(bool state)
    {
        createRandomRoomBtn.interactable = state;
    }

    public void SetCreateRoomBtnStateReverse(bool state)
    {
        createRandomRoomBtn.interactable = !state;
    }

    public void OnBeforeJoiningLobby()
    {
        SetReturnToMenuBtnState(false);
    }
    public void OnAfterJoiningLobby(bool state)
    {
        SetCreateRoomBtnState(state);
        SetReturnToMenuBtnState(true);
    }
    public void OnBeforeJoiningARoom()
    {
        SetSessionListJoinBtnState(false);
        SetReturnToMenuBtnState(false);
        SetCreateRoomBtnState(false);
    }
    public void OnAfterJoiningARoom(bool state)
    {
        if (!state)
        {
            SetSessionListJoinBtnState(true);
            SetReturnToMenuBtnState(true);
            SetCreateRoomBtnState(true);
        }
    }

    public void SetReturnToMenuBtnState(bool state)
    {
        returnToMenuBtn.interactable = state;
    }

    protected void OnReturnToMenuBtnClicked()
    {
        LobbyManager.instance.ReturnToMenu();
    }

    protected void OnCreateRandomRoomBtnClicked()
    {
        LobbyManager.instance.CreateNewRandomRoom();
        SetReturnToMenuBtnState(false);
        SetCreateRoomBtnState(false);
    }

    protected void OnPlayerNameChanged(string value)
    {
        GameManager.instance.SetLocalPlayerName(value);
    }
    protected void CreateRoomSessionUI(SessionInfo sessionInfo)
    {
        if (!roomSessionEntryUIPrefab)
            return;

        var go = Instantiate(roomSessionEntryUIPrefab);
        go.transform.SetParent(entriesScrollContentParent);
        go.transform.localScale = Vector3.one;

        go.Init();
        go.SetRoomName(sessionInfo.Name);
        go.SetRoomSize(sessionInfo.PlayerCount + "/" + sessionInfo.MaxPlayers);

        roomSessionEntryUIs.Add(go);
    }

    protected void UpdateSessionUI(SessionInfo session, RoomSessionEntryUI found)
    {
        if (!session.IsValid)
            roomSessionEntryUIs.Remove(found);
    }

    
}
