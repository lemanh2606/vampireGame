using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health;

    public void TakeDamege(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
