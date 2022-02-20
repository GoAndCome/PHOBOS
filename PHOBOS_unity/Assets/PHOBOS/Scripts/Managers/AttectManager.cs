using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttectManager : MonoBehaviour
{
	public static AttectManager instance;
	private CameraShake cameraShake;

	public int iAttackPower = 50;

	public void Start()
	{
		cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
	}
	public void Attack(int nindex)
	{

		switch (nindex)
		{
			case 0:
				WeakAttack();
				break;

			case 1:
				normalAttack();
				break;

			case 2:
				StrongAttack();
				break;
		}
	}

	public void WeakAttack()
	{
		cameraShake.ShakeTime = 0.05f;

		GUIManager.instance.ACounting = false;
		GUIManager.instance.AttackCounterNow = GUIManager.instance.PatternCheck;
		GUIManager.instance.PlayerHP -= iAttackPower * 10; //500
		GUIManager.instance.PlayerMP += iAttackPower;
		if(!GUIManager.instance.BSanchipinchi)
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 5;
		} else
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 1;
		}
		
		if (GUIManager.instance.PlayerSAN < 0)
		{
			GUIManager.instance.PlayerSAN = 0;
		}
		GUIManager.instance.AttackControl();
	}
	public void normalAttack()
	{
		cameraShake.ShakeTime = 0.1f;

		GUIManager.instance.ACounting = false;
		GUIManager.instance.AttackCounterNow = GUIManager.instance.PatternCheck;
		GUIManager.instance.PlayerHP -= iAttackPower*20; //1000
		GUIManager.instance.PlayerMP += iAttackPower*2;
		if (!GUIManager.instance.BSanchipinchi)
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 6;
		}
		else
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 1;
		}
		if (GUIManager.instance.PlayerSAN < 0)
		{
			GUIManager.instance.PlayerSAN = 0;
		}
		GUIManager.instance.AttackControl();
	}

	public void StrongAttack()
	{
		cameraShake.ShakeTime = 0.15f;

		GUIManager.instance.ACounting = false;
		GUIManager.instance.AttackCounterNow = GUIManager.instance.PatternCheck;
		GUIManager.instance.PlayerHP -= iAttackPower*30;
		GUIManager.instance.PlayerMP += iAttackPower*3;
		if (!GUIManager.instance.BSanchipinchi)
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 5;
		}
		else
		{
			GUIManager.instance.PlayerSAN -= iAttackPower * 1;
		}
		if (GUIManager.instance.PlayerSAN < 0)
		{
			GUIManager.instance.PlayerSAN = 0;
		}
		GUIManager.instance.AttackControl();
	}

}
