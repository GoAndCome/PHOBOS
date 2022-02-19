using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTestScript : MonoBehaviour
{
    public void OnCilckButtonSanZero()
    {
        GUIManager.instance.PlayerSAN = 0;
    }

    public void OnCilckButtonSanFull()
    {
        GUIManager.instance.PlayerSAN = GUIManager.instance.playerMaxSAN;
    }

    public void OnCilckButtonGameWin()
    {
        GUIManager.instance.EnemyHP = 0;
    }
}
