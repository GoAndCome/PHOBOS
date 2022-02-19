using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillAnim : MonoBehaviour
{
    public static UseSkillAnim instance;

    Animator animator;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        //gameObject.GetComponentsInParent
    }

    public void SkillColdTime()
    {
        animator.Play("UseSkillAnim");
    }

    public void SkillReUse()
    {
        animator.Play("UseSkillAnimAppear");
    }
}
