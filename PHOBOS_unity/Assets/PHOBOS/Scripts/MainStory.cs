using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainStory : MonoBehaviour
{

    public GameObject BackGround_obj;

    public Text testText_txt;

    public Sprite BackGroundStart_spr;
    public Sprite BackGroundEnd_spr;

    string [] testStartText_str = new string[10];
    string [] testEndText_str = new string[10];

    int textNumber;

    bool THXClick = false;

    void Start()
    {
        BackGround_obj.GetComponent<Image>().sprite = null; 

        textNumber = 0;

        for (int i = 0; i < testStartText_str.Length; i++)
        {
            testStartText_str[i] = null;
        }

        testStartText_str[0] = "처음 나오는 대사";
        testStartText_str[1] = "난 어두운 숲을 걷고 있다.";
        testStartText_str[2] = "머리 위에선 까마귀 우는 소리와 불길한 날갯짓 소리가 들리고 주변에선 거친 숨소리가 들려온다.";
        testStartText_str[3] = "이 위험한 곳에 들어온 이유는 단 하나, 이 숲 너머에 모든 저주를 풀 수 있다는 마법사가 있단 소문...";
        testStartText_str[4] = "내 팔에 걸린 저주를 풀기 위해 스스로 이 어두운 숲에 걸어 들어왔다.";
        testStartText_str[5] = "살아남아서 이 숲을 빠져나가고 저주를 풀 것이다...";

        for (int i = 0; i < testEndText_str.Length; i++)
        {
            testEndText_str[i] = null;
        }

        testEndText_str[0] = "달빛이 가리키는 곳에 숲의 출구가 보인다.";
        testEndText_str[1] = "달빛이 가리키는 곳에 숲의 출구가 보인다.";
        testEndText_str[2] = "나는 이 숲에서 살아남은 것이다...";
        testEndText_str[3] = "안도감과 승리감을 안고 달빛을 따라간다.";
        testEndText_str[4] = "앞으로 어떤 일이 벌어질지 모른 채... ";

        ClickStartStory();

        if (PlayerPrefs.GetInt("OneGameStart_i") <= 0)
        {
            ClickStartStory();
        } else if (PlayerPrefs.GetInt("OneGameStart_i", 0) >= 3)
        {
            ClickEndStory();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (PlayerPrefs.GetInt("OneGameStart_i") <= 0)
            {
                ClickStartStory();
            }
            else if (PlayerPrefs.GetInt("OneGameStart_i", 0) >= 3)
            {
                ClickEndStory();
            }
        }
    }

    void ClickStartStory()
    {
        BackGround_obj.GetComponent<Image>().sprite = BackGroundStart_spr;

        if (textNumber < testStartText_str.Length && testStartText_str[textNumber] != null)
        {
            testText_txt.text = testStartText_str[textNumber];
        } else
        {
            int a = 1;
            PlayerPrefs.SetInt("OneGameStart_i", a);
            Debug.Log("OneGameStart_i : " + PlayerPrefs.GetInt("OneGameStart_i"));

            gameObject.SetActive(false);
            BackGround_obj.SetActive(false);
        }
        textNumber++;
    }

    void ClickEndStory()
    {
        BackGround_obj.GetComponent<Image>().sprite = BackGroundEnd_spr;

        if (textNumber < testEndText_str.Length && testEndText_str[textNumber] != null)
        {
            testText_txt.text = testEndText_str[textNumber];
        }
        else
        {
            if (!THXClick)
            {
                GameManager.instance.LoadScene("ThankYou");
            }
            THXClick = true;
        }
        textNumber++;
    }
}
