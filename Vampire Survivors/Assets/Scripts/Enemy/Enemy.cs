using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject destroyEffect;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        direction = (PlayerController.Instance.transform.position -
            transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x*moveSpeed, direction.y*moveSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(destroyEffect,transform.position,transform.rotation);
        }
    }
}
