using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using System;

enum AttackPatten
{
	WEAK = 0,
	NORMAL,
	STRONG
}

public class GUIManager : MonoBehaviour {
	public static GUIManager instance;

	public GameObject gameOverPanel;
	public GameObject EnemyHPBar;
	public GameObject playerHPBar;
	public GameObject playerMPBar;
	public GameObject playerSANBar;
	public GameObject gamePause;
	public GameObject SanchipinchiPanel;
	public GameObject test_cooltimeSee;
	//public GameObject EnemyAttackIcon;
	public GameObject EndingWin;
	public GameObject EndingLose; //게임이 끝난 후 승리/패배를 표시하는 텍스트
								  //public GameObject ComboPrefab;

	public bool BCombo = false;
	public bool ACounting = false;
	public bool BSanchipinchi = false;

	public Text AttackCountTxt;
	public Text moveCounterTxt;
	public Text ComboTxt;

	public int PatternCheck;
	public int playerMaxHP; //플레이어 최대 체력 
	public int playerMaxMP; //플레이어 최대 MP
	public int playerMaxSAN;
	public int enemyMaxHP; //적 최대 체력
	public int AttackCounterNow = 99999;
	public int StageClearNumber;

	public float addComboTime = 1f; //콤보 텍스트가 없어질 때까지 대기하는 시간 

	private int playernowhp; //플레이어 현재 체력
	private int enemynowhp;
	private int mp;
	private int san;
	private int moveCounter = 20;
	private int AttackCounter_i = 999;
	private int CoolTimeCount_H = 0;
	private int CoolTimeCount_A = 0;
	private int CoolTimeCount_M = 0;
	private int combo;

	public int PlayerHP
	{
		get
		{
			return playernowhp;
		}
		set
		{
			playernowhp = value; //현재 체력이 변경될 때마다 
			StartCoroutine(PlayerHPControl()); //EnemyHPControl 함수를 호출해서 화면의 체력바 컨트롤
		}
	}

	public int EnemyHP
	{
		get
		{
			return enemynowhp; 
		}
		set
		{
			enemynowhp = value; //현재 체력이 변경될 때마다 
			StartCoroutine(EnemyHPControl()); //EnemyHPControl 함수를 호출해서 화면의 체력바 컨트롤
		}
	}

	public int PlayerMP
	{
		get
		{
			return mp;
		}
		set
		{
			mp = value;
			PlayerMPControl();
			//값이 변함
		}
	}

	public int PlayerSAN
	{
		get
		{
			return san;
		}
		set
		{
			san = value;
			PlayerSANControl();
			//값이 변함
		}
	}


	public int ComboCount
	{
		get
		{
			return combo;
		}
		set
		{
			combo = value;

			if (combo > 2) //최소 콤보가 2개 이상일 경우에만 화면에 표시
			{ 
				//addComboTime += 0.5f; //콤보가 쌓일 때마다 사라질 때까지 대기하는 시간 증가
				StartCoroutine(ComboControl()); //콤보 처리 함수 호출 
			}
		}
	}

	
	public int MoveCounter
	{
		get
		{
			return moveCounter;
		}
		set
		{
			if (moveCounter < value) //늘어난 경우
			{
				moveCounter = value; 
				
				moveCounterTxt.text = moveCounter.ToString();
				
			} else //같은 값이 들어오거나 줄어든 경우
			{
				moveCounter = value; //moveCounter의 값이 0이 될 경우 (value값을 0으로 받을 경우) 
				if (moveCounter <= 0)
				{
					moveCounter = 0;
					StartCoroutine(WaitForShifting());
				}
				moveCounterTxt.text = moveCounter.ToString();

				Invoke("MoveCountActive", 0.8f);

			}
		}
	}

	public void MoveCountActive()
	{
		if (CoolTimeCount_H >= 1) //스킬 쿨타임이 1 이상 있는 경우
		{
			CoolTimeCount_H -= 1;
		}
		if (CoolTimeCount_A >= 1) //스킬 쿨타임이 1 이상 있는 경우
		{
			CoolTimeCount_A -= 1;
			if (CoolTimeCount_A <= 0)
			{
				//UseSkillAnim.instance.SkillReUse();
			}
		}
		if (CoolTimeCount_M >= 1) //스킬 쿨타임이 1 이상 있는 경우
		{
			CoolTimeCount_M -= 1;
		}

		AttackCounter_i -= 1;
		AttackControl();
	}

	public void Start()
	{
		AttackControl();
		PlayerHP = playerMaxHP;
		EnemyHP = enemyMaxHP;
		PlayerMP = 0;
		PlayerSAN = playerMaxSAN;
		ComboCount = 0;


	}

	void Awake() 
	{
		moveCounterTxt.text = moveCounter.ToString();
		instance = GetComponent<GUIManager>();
	}

	// Show the game over panel
	public void GameOver(int stageValue) 
	{
		GameManager.instance.gameOver = true;

		gameOverPanel.SetActive(true);

		if (EnemyHP <= 0)
		{
			EndingLose.SetActive(false);
			EndingWin.SetActive(true);
			PlayerPrefs.SetInt("OneGameStart_i", stageValue);
			Debug.Log("OneGameStart_i : " + PlayerPrefs.GetInt("OneGameStart_i"));
		}
		else if(MoveCounter <= 0 || PlayerHP <= 0)
		{
			EndingWin.SetActive(false);
			EndingLose.SetActive(true);
		}
	}

	private IEnumerator WaitForShifting()
	{
		yield return new WaitUntil(()=> !BoardManager.instance.IsShifting); //특정 식이 참이 될 때까지 기다림
		//이 경우 BoardManager에서 IsShiftIng이 false가 될 경우 (= 매칭이 모두 끝날 경우) 까지 일시정지
		yield return new WaitUntil(() => !BCombo);
		yield return new WaitForSeconds(.25f);
		GameOver(StageClearNumber); //매칭이 모두 끝나고 0.25초를 기다린 후 GameOver 함수 불러오기
	}
	/*
	public void HighScoreDelete () //HighScore 초기화용 함수
	{
		PlayerPrefs.DeleteKey("HighScore"); //버튼을 누를 경우 HighScore 값 제거
	}*/

	public IEnumerator EnemyHPControl() //패널이 매치될 경우 적의 체력을 컨트롤
	{
		EnemyHPBar.GetComponent<Image>().fillAmount = EnemyHP / (float)enemyMaxHP;
		//현재 체력 / 전체 체력 (퍼센트로 바꾸는 과정)값을 fillAmount에 넣어 체력바가 줄어들게 함.
		if(EnemyHP <= 0)
		{
			yield return new WaitUntil(() => !BoardManager.instance.IsShifting); //특정 식이 참이 될 때까지 기다림
			yield return new WaitUntil(() => !BCombo);
			yield return new WaitForSeconds(.50f);
			GameOver(StageClearNumber); //현재 체력이 0 이하가 될 경우 GameOver 함수를 불러옴.
		}
	}

	public IEnumerator PlayerHPControl()
	{
		playerHPBar.GetComponent<Image>().fillAmount = PlayerHP / (float)playerMaxHP;
		//현재 체력 / 전체 체력 (퍼센트로 바꾸는 과정)값을 fillAmount에 넣어 체력바가 줄어들게 함.
		if (PlayerHP <= 0)
		{
			yield return new WaitUntil(() => !BoardManager.instance.IsShifting); //특정 식이 참이 될 때까지 기다림
			yield return new WaitUntil(() => !BCombo);
			yield return new WaitForSeconds(.50f);
			GameOver(StageClearNumber); //현재 체력이 0 이하가 될 경우 GameOver 함수를 불러옴.
		}
	}

	public void PlayerMPControl() //패널이 매치될 경우 플레이어의 MP를 컨트롤
	{
		playerMPBar.GetComponent<Image>().fillAmount = PlayerMP / (float)playerMaxMP;
	}

	public void PlayerSANControl()
	{
		Debug.Log("현재SAN : " + PlayerSAN);
		playerSANBar.GetComponent<Image>().fillAmount = PlayerSAN / (float)playerMaxSAN;
		if (PlayerSAN <= (playerMaxSAN * 0.1)&&!BSanchipinchi)
		{
			PlayerSanchipinchiStart();
			BSanchipinchi = true;
		}
		if (PlayerSAN >= (playerMaxSAN * 0.5)&&BSanchipinchi) 
		{
			PlayerSanchipinchiStop();
			BSanchipinchi = false;
		}
	}

	public IEnumerator ComboControl()
	{
		BCombo = true;

		yield return new WaitUntil(() => !BoardManager.instance.IsShifting);
		if (ComboCount >= 2)
		{
			ComboTxt.gameObject.SetActive(true);
			ComboTxt.text = ComboCount + " Combo!";

			yield return new WaitForSeconds(addComboTime);

			ComboCount = 0; //콤보 수 0으로 초기화

			ComboTxt.gameObject.SetActive(false); //텍스트 비활성화
		} 
		else
		{
			ComboCount = 0;
		}
		BCombo = false;
	}

	public void CallPasueButton()
	{
		gamePause.SetActive(true); //일시정지 창 열기
	}

	public void TestButtonC ()
	{
		gamePause.SetActive(false); //일시정지 창 닫기 (게임으로 돌아가기) 
	}
	public void TestButtonQ()
	{
		Debug.Log("2버튼 눌림");
	}

	//적 공격 관련
	public void AttackControl()
	{
		AttectManager attectManager = GameObject.Find("EnemyAttack").GetComponent<AttectManager>();
		if (!ACounting) //패턴이 끝나서 카운팅(공격 턴세기)을 하지 않고 있을 경우
		{ // ==false 일 경우
			while (true)
			{
				PatternCheck = UnityEngine.Random.Range(1, 30000) % 3; //사용할 패턴 정하기
				if (AttackCounterNow != PatternCheck) //바로 전과 동일한 패턴이 뽑혔는가?
				{
					break; //바로 전과 다른 패턴이 뽑했을 경우 반복문을 빠져나감
				} //같은 패턴이 뽑혔을 경우 다른 패턴이 뽑힐 때까지 반복
			} 

			ACounting = true; //카운팅 시작
			AttackCounter_i = PatternCheck + 1; //최소 카운트가 1이 되도록 1을 더해줌

		} else // ==true 일 경우
		{
			if (AttackCounter_i <= 0) //카운팅이 끝났을 경우 (어택 카운터가 0 이하가 됐을 경우)
			{
				attectManager.Attack(PatternCheck); //패턴체커에 따라 공격함
				SFXManager.instance.PlaySFX(Clip.Atteck);
			}
			
		}
		switch (PatternCheck) //화면에 텍스트 출력 
		{
			case 0:
				//EnemyAttackIcon.GetComponent<Image>().sprite = Resources.Load("공격카운트_임시이미지", typeof(Sprite)) as Sprite;
				//EnemyAttackIcon.GetComponent<Image>().sprite = null;

				AttackCountTxt.text = "WEAK: " + AttackCounter_i.ToString();
				break;
			case 1:
				//EnemyAttackIcon.GetComponent<Image>().sprite = Resources.Load("공격카운트_임시이미지", typeof(Sprite)) as Sprite;
				//EnemyAttackIcon.GetComponent<Image>().sprite = null;

				AttackCountTxt.text = "NORMAL: " + AttackCounter_i.ToString();
				break;
			case 2:
				//EnemyAttackIcon.GetComponent<Image>().sprite = Resources.Load("공격카운트_임시이미지", typeof(Sprite)) as Sprite;
				//EnemyAttackIcon.GetComponent<Image>().sprite = null;
				AttackCountTxt.text = "STRONG: " + AttackCounter_i.ToString();
				break;
		}

	}

	

	public void UseHealingSkill(/*int nCoolTime*/)
	{
		if (CoolTimeCount_H <= 0)
		{
			Debug.Log("회복 스킬 사용");
			if (PlayerHP < playerMaxHP)
			{
				if (playerMaxHP - PlayerHP < 1000) //회복 가능한 HP가 1000초과
				{
					PlayerHP += playerMaxHP - PlayerHP; //MaxHP를 넘겨 회복하지 않게 하기 위함
				}
				else
				{//회복 가능한 HP가 1000이상
					PlayerHP += 1000;
				}
				CoolTimeCount_H = 2;
			}
		} else
		{
			test_cooltimeSee.SetActive(false);
			test_cooltimeSee.SetActive(true);
			Debug.Log("회복 스킬 쿨타임 : " + CoolTimeCount_H);
		}
	}

	public void UseAttackCountUPSkill()
	{
		if (CoolTimeCount_A <= 0)
		{
			Debug.Log("공격 카운트 증가 스킬 사용");
			AttackCounter_i += 1;
			CoolTimeCount_A = 3;
			//UseSkillAnim.instance.SkillColdTime();
			AttackControl();
		} else
		{
			test_cooltimeSee.SetActive(false);
			test_cooltimeSee.SetActive(true);
			Debug.Log("어택카운트 스킬 쿨타임 : " + CoolTimeCount_A);
		}
	}

	public void UseMoveCountUPSkill()
	{
		if (CoolTimeCount_M <= 0)
		{
			Debug.Log("이동 카운트 증가 스킬 사용");
			MoveCounter += 1;
			CoolTimeCount_M = 3;
		}
		else
		{
			test_cooltimeSee.SetActive(false);
			test_cooltimeSee.SetActive(true);
			Debug.Log("어택카운트 스킬 쿨타임 : " + CoolTimeCount_M);
		}
	}

	public void PlayerSanchipinchiStart()
	{
		SFXManager.instance.PlaySFX(Clip.HeartBeat);
		SanchipinchiPanel.SetActive(true);
	}
	public void	PlayerSanchipinchiStop()
	{
		SFXManager.instance.StopSFX(Clip.HeartBeat);
		SanchipinchiPanel.SetActive(false);
	}

}
