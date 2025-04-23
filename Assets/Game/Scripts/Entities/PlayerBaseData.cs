using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBaseData
{
    private uint networkId;
    private string name;
    public PlayerBaseData(uint networkID, string name)
    {
        networkId = networkID;
        this.name = name;
        OnNameChanged = null;
    }
    public uint NetworkID
    {
        get { return networkId; }
        protected set { networkId = value; }
    }

    public string Name
    {
        get { return name; }
        protected set
        { name = value;  }
    }

    public void SetName(string value)
    {
        Name = value;
        OnNameChanged?.Invoke(value);
    }

    public System.Action<string> OnNameChanged;
}
