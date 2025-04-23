using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;

    public static RoomManager instance;

    private void Awake()
    {
        instance = this;
        UIManager.Instance.OnEnterRoom();
    }
    public void ReturnToLobby()
    {
        NetworkRoomManager.instance.OnReturnToLobby();
    }
}
