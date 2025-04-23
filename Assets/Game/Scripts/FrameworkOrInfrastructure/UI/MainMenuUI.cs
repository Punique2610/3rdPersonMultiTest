using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimUI.ModernMenu;
public class MainMenuUI : UIMenuManager
{
    [SerializeField] private Button enterLobbyBtn;

    public void Init()
    {
        if (enterLobbyBtn)
        {
            enterLobbyBtn.onClick.RemoveListener(OnEnterLobbyBtnClicked);
            enterLobbyBtn.onClick.AddListener(OnEnterLobbyBtnClicked);
        }
    }

    public void OnEnterLobbyBtnClicked()
    {
        GameManager.instance.EnterLobby();
    }

    public void OnStartLoadNewScene()
    {
        mainCanvas.SetActive(false);
        loadingMenu.SetActive(true);
    }

    public void SetProgressBar(float val)
    {
        loadingBar.value = val;
    }
}
