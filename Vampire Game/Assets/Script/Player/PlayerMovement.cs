using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    [HideInInspector]
    public Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        if (moveInput.sqrMagnitude > 1)
        {
            moveInput = moveInput.normalized;
        }
        //flip
        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(moveInput.x > 0 ? 1 : -1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        Debug.Log("Move Input: " + moveInput);
    }
}
