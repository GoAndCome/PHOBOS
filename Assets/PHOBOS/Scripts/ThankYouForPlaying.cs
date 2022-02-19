using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThankYouForPlaying : MonoBehaviour
{
    float MoveTime;
    bool move;
    void Start()
    {
        MoveTime = 0;
        move = true;
    }

    void Update()
    {
        MoveTime += Time.deltaTime;
        if (move)
        {
            if (Input.GetMouseButtonDown(1) || MoveTime >= 20.0f)
            {
                GameManager.instance.LoadScene("Menu");
                move = false;
            }
        }
    }
}
