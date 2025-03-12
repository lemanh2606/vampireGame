using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;


    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindFirstObjectByType<PlayerMovement>().transform;
    }

    void Update()
    {

        // if we are currenly being knoched back, then the knockback
        if(knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            // Otherwise, constantly move the enemy towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);    //Constantly move the enemy towards the player   
        }


        
      
    }


    // This is meant to be called form other scripts to create knockback.

    public void Knockback(Vector2 velocity, float duration)
    {
       if(knockbackDuration > 0)
        {
            knockbackVelocity = velocity;
            knockbackDuration = duration;
        }
    }
}