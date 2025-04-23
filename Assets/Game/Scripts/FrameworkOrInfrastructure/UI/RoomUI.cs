using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private Button returnToLobbyBtn;
    [SerializeField] private PlayerHeadUI playerHeadUI;

    public void Init()
    {
        if (returnToLobbyBtn)
        {
            returnToLobbyBtn.onClick.RemoveListener(OnReturnToLobbyBtnClicked);
            returnToLobbyBtn.onClick.AddListener(OnReturnToLobbyBtnClicked);
        }
    }

    public void OnReturnToLobbyBtnClicked()
    {
        RoomManager.instance.ReturnToLobby();
    }

    public void OnNewPlayerSpawned(PlayerBaseData playerBasicInfo, Transform playerGo)
    {
        var headUI = Instantiate(playerHeadUI);

        headUI.transform.parent = playerGo.transform;
        headUI.transform.localPosition = Vector3.zero;

        playerBasicInfo.OnNameChanged -= headUI.SetPlayerName;
        playerBasicInfo.OnNameChanged += headUI.SetPlayerName;
    }

    
}
