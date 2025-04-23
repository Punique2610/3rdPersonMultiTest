using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHeadUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt;
    
    public void SetPlayerName(string val)
    {
        playerNameTxt.text = val;
    }
}
