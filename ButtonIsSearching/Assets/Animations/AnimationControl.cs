using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;

    float testtimer;
    int anim = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        
        
        
        //only for visualisation 
        testtimer -= Time.deltaTime;
        if(testtimer < 0)
        {
            anim++;
            testtimer = 3f;
        }
        
        if (anim == 0)
        {
            moveRightUpwards();
            print(anim);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if(anim == 1)
        {
            moveRightSideways();
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (anim == 2)
        {
            moveRightDownwards();
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (anim == 3)
        {
            moveLeftDownwards();
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (anim == 4)
        {
            moveLeftSideways();
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (anim == 5)
        {
            moveLeftUpwards();
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (anim == 6)
        {
            anim = 0;
        }
    }*/



}
