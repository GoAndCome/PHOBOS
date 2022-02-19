using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class WorldMap : MonoBehaviour
{
    public GameObject P_Stage;
    public GameObject SD_Char_obj;
    public GameObject SelectFalse;
    public GameObject Story_obj;
    public GameObject[] Pre_Born_White =  new GameObject[3];

    public GameObject Stage1Button;
    public GameObject Stage2Button;
    public GameObject Stage3Button;

    public Sprite SD_Char_spr;
    public Sprite StageActiveTrue_spr;
    public Sprite StageActiveFalse_spr;

    public Text T_StageName;
    public Text StageExplanation_txt;

    GameObject[] StageButtons = new GameObject[3]; //스테이지 3개

    public void Start()
    {
        GameManager.instance.gameOver = false;

        Debug.Log("OneGameStart_i : " + PlayerPrefs.GetInt("OneGameStart_i"));

        if (StageButtons[0] != GameObject.Find("-1"))
        {
            StageButtons[0] = GameObject.Find("-1"); // 1-1
            StageButtons[1] = GameObject.Find("-2"); // 1-2
            StageButtons[2] = GameObject.Find("-3"); // 1-3
        }

        for(int i = 0; i < StageButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("OneGameStart_i") == 0)
            { //처음 스테이지 화면에 진입했을 때 1-1은 무조건 활성화하는 if문
                //한번 실행되면 더 실행되지 않음(실행되어도 상관 없음)
                StageButtons[0].GetComponent<Image>().sprite = StageActiveTrue_spr;
                Change_Cha(StageButtons[0]);
            }
            if (i < PlayerPrefs.GetInt("OneGameStart_i"))
            {
                StageButtons[i].GetComponent<Image>().sprite = StageActiveTrue_spr;
                if(i > 0)
                {
                    Pre_Born_White[i - 1].SetActive(true);
                }
                Change_Cha(StageButtons[i]);
            } else
            {
                StageButtons[i].GetComponent<Image>().sprite = StageActiveFalse_spr;
            }
        }

        if (PlayerPrefs.GetInt("OneGameStart_i") <= 0 || PlayerPrefs.GetInt("OneGameStart_i") >= 4)
        {
            Story_obj.SetActive(true);
        }
//        P_Stage = GameObject.Find("Stage_Select_Panel");
    }

    public void OnClickStage(string sValue)
    {
        P_Stage.SetActive(false);

        if (sValue == "초입")
        {
            if(StageButtons[0].GetComponent<Image>().sprite == StageActiveTrue_spr)
            {
                Change_Cha(StageButtons[0]);
                StageExplanation_txt.text = "어두운 나무 위에서\n붉은 빛이 번쩍인다...";
                P_Stage.SetActive(true);

                Stage1Button.SetActive(true);
                Stage2Button.SetActive(false);
                Stage3Button.SetActive(false);
            } else
            {
                NonSelcetStage();
            }
        }
        else if (sValue == "중부")
        {
            if (StageButtons[1].GetComponent<Image>().sprite == StageActiveTrue_spr)
            {
                Change_Cha(StageButtons[1]);
                StageExplanation_txt.text = "저 멀리서 땅을 울리는\n발소리가 다가온다...";
                P_Stage.SetActive(true);

                Stage1Button.SetActive(false);
                Stage2Button.SetActive(true);
                Stage3Button.SetActive(false);
            }
            else
            {
                NonSelcetStage();
            }
        }
        else if (sValue == "출구")
        {
            if (StageButtons[2].GetComponent<Image>().sprite == StageActiveTrue_spr)
            {
                Change_Cha(StageButtons[2]);
                StageExplanation_txt.text = "드디어 숲의 끝이 보인다.\n흉폭하고 거대한 그림자도...";
                P_Stage.SetActive(true);

                Stage1Button.SetActive(false);
                Stage2Button.SetActive(false);
                Stage3Button.SetActive(true);
            }
            else
            {
                NonSelcetStage();
            }
        } else
        {
            StageExplanation_txt.text = "error!";
        }

        T_StageName.text = sValue;
        
    }

    public void OnClickStageCancel()
    {
        P_Stage.SetActive(false);
    }

    public void Change_Cha(GameObject stage_button)
    {
        if (SD_Char_obj != null)
        {
            Vector2 Cha_posistion = new Vector2(stage_button.transform.position.x - 7f, stage_button.transform.position.y + 8f);
            SD_Char_obj.transform.position = Cha_posistion;
        }
    }
    
    public void NonSelcetStage()
    {
        SelectFalse.SetActive(false);
        SelectFalse.SetActive(true);
    }

}
