using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.moveInput.x !=0 || pm.moveInput.y != 0)
        {
            am.SetBool("move", true);
           // SpriteDirectionChecker();

        }
        else
        {
            am.SetBool("move", false);
        }
    }

    //void SpriteDirectionChecker()
    //{
    //    if (pm.moveInput.x < 0)
    //    {
    //        sr.flipX = true;
    //    }
    //    else
    //    {
    //        sr.flipX = false;
    //    }
    //}
}
