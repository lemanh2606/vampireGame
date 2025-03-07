using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;


    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // What the color of the damage flash should be .
    public float damageFlashDuration = 0.2f; // How long the  flash should last.
    public float deathFadeTime = 0.6f; // How mch time it takes for the enemy to fade.
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;
    void Awake()
    {
        //Assign the vaiables
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

     void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }
     void Update()
    {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {

        currentHealth -= dmg;
        StartCoroutine(DamagerFlash());

        // Create the text popup when enemy takes damage
        if(dmg > 0)
        {
         GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        // Apply knockback if it not zero.
        if (knockbackForce > 0)
        {
            // Get the direction of knockback.
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce , knockbackDuration);
        }

        // Kills the enemy if th heath drops below zero.
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    IEnumerator DamagerFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }


    // This is a coroutine funtion tha fade the enemy away slowly
    IEnumerator KillFade()
    {
        // Wait for a siglle frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        // This is a loop that fires every frame .
        while (t < deathFadeTime)
        {


            yield return w;
            t += Time.deltaTime;

            // Set the colour for this frame
        }
        Destroy(gameObject);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0,es.relativeSpawnPoints.Count)].position;
    }
}