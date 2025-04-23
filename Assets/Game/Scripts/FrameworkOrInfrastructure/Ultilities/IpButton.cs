using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IpButton
{
    public void Set(int buttonID, bool state);
    public void Set(int buttonID);
    public void Set(bool state);

}
