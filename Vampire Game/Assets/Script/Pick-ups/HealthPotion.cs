using UnityEngine;

public class HealthPotion : Pickup,ICollectible
{
    public int healthToRestore;
    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
       
    }
}
