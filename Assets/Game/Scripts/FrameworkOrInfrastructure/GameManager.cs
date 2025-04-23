using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform localPlayerObject;
    public PlayerBaseDataServer thisPlayerServerData { get; protected set; }
    public Dictionary<PlayerBaseData, Transform> currentPlayersDict { get; private set; }

    public System.Action<PlayerBaseData, Transform> OnNewPlayerSpawned;

    private void Awake()
    {
        var dups = FindObjectsOfType<GameManager>();

        foreach(var dup in dups)
            if(dup != this)
                Destroy(dup.gameObject);

        instance = this;
        DontDestroyOnLoad(this);
      
        thisPlayerServerData = new PlayerBaseDataServer("PlaceholderID", PlayerPrefs.GetString("PlayerLocalName", this.GetInstanceID().ToString()));

        if (currentPlayersDict == null)
            currentPlayersDict = new Dictionary<PlayerBaseData, Transform>();
    }
    public void EnterLobby()
    {
        pSceneManager.instance.LoadNewSceneAsync("LobbyScene");
        //pSceneManager.instance.LoadNewSceneWithDefaultPath("LobbyScene.Unity");
    }
    public void OnNewPlayerJoined()
    {
        var allPlayersObjects = FindObjectsOfType<PlayerNetworkCompositeRoot>();
        foreach (var player in allPlayersObjects)
        {
            var playerNO = player.GetComponent<NetworkObject>();

            if (!GetPlayerObjectFromNetworkID(playerNO.Id.Raw))
            {
                var playerBaseData = new PlayerBaseData(player.GetComponent<NetworkObject>().Id.Raw, "name");

                currentPlayersDict.Add(playerBaseData, player.transform);

                var networkPropertiesUpdator = player.GetComponent<PlayerNetworkedPropertiesUpdator>();
                networkPropertiesUpdator.SetThisPlayerBaseData(playerBaseData);

                // first init data sync
                if (NetworkRoomManager.instance.Runner.LocalPlayer == player.Runner.LocalPlayer)
                {
                    playerBaseData.SetName(thisPlayerServerData.Name);
                    localPlayerObject = player.transform;
                }

                OnNewPlayerSpawned?.Invoke(playerBaseData, player.transform);
                //Init assign
                networkPropertiesUpdator.AssignNetworkedPlayerBaseDataToLocal();
            }
        }    
    }
    public void OnAPlayerLeft()
    {
        var keysToRemove = new List<PlayerBaseData>();
        foreach (var player in currentPlayersDict)
        {
            if (player.Value == null)
                keysToRemove.Add(player.Key);
        }

        foreach (var key in keysToRemove)
            currentPlayersDict.Remove(key);
    }
    public void SetLocalPlayerName(string value)
    {
        thisPlayerServerData.Name = value;
        SaveLocalPlayerData();
    }
    public void SaveLocalPlayerData()
    {
        PlayerPrefs.SetString("PlayerLocalName", thisPlayerServerData.Name);
        PlayerPrefs.Save();
    }
    public void RemovePlayerObjectByNetworkID(uint playerNetworkID)
    {
        foreach (var key in currentPlayersDict.Keys)
        {
            if (key.NetworkID == playerNetworkID)
                currentPlayersDict.Remove(key);
        }
    }
    public Transform GetPlayerObjectFromNetworkID(uint playerNetworkID)
    {
        foreach (var key in currentPlayersDict.Keys)
        {
            if (key.NetworkID == playerNetworkID)
                return currentPlayersDict[key];
        }
        return null;
    }
    public Transform GetPlayerObjectFromBaseData(PlayerBaseData playerBaseData)
    {
        currentPlayersDict.TryGetValue(playerBaseData, out var playerOb);
        return playerOb;
    }

    
}
