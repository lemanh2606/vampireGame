using UnityEngine;

public class hammerSawBehaviour : ProjectileWeaponBehaviour
{

    hammerSawController hs;
    void Start()
    {
        base.Start();
        hs = FindAnyObjectByType<hammerSawController>();
    }

  
    void Update()
    {
        transform.position += direction * weaponData.Speed * Time.deltaTime; // Set the movement of the hammer Saw
    }
}
