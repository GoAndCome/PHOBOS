using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static EnemyAttackManager instance;

    public int AttackCounter_i;
    public void Start()
    {
        AttackCounter_i = 3;
    }
    public void callAttackCounter()
    {
        AttackCounter_i -= 1;
    }
    private void AttackControl()
    {
        if(AttackCounter_i <= 0)
        {
            GUIManager.instance.PlayerHP = GUIManager.instance.PlayerHP - 50;
        }
    }
}
