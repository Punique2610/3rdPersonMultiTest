using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSessionEntryUI : MonoBehaviour
{
    [SerializeField] private Button joinBtn;
    [SerializeField] private TMP_Text roomNameTxt;
    [SerializeField] private TMP_Text roomSizeTxt;
    public string RoomName { get { return roomNameTxt.text; } protected set { } }

    public void Init()
    {
        if (joinBtn)
        {
            joinBtn.onClick.RemoveListener(OnJoinBtnClicked);
            joinBtn.onClick.AddListener(OnJoinBtnClicked);
        }
    }

    public void SetJoinBtnState(bool state)
    {
        if(joinBtn)
            joinBtn.interactable = state;
    }

    public void OnJoinBtnClicked()
    {
        LobbyManager.instance.JoinRoom(RoomName);
        SetJoinBtnState(false);
    }

    public void SetRoomName(string name)
    {
        RoomName = name;
        roomNameTxt.text = name;
    }

    public void SetRoomSize(string name)
    {
        roomSizeTxt.text = name;
    }
}
