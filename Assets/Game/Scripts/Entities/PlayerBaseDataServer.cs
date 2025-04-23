using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDataServer 
{
    private string serverID;
    private string severName;

    public PlayerBaseDataServer(string serverID, string severName)
    {
        this.serverID = serverID;
        this.severName = severName;
    }

    public string ServerID
    {
        get { return serverID; }
        set { serverID = value; }
    }

    public string Name
    {
        get { return severName; }
        set
        {
            severName = value;
        }
    }
}
