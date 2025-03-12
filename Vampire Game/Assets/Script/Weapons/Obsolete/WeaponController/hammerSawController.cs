using UnityEngine;

public class hammerSawController : WeaponController 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
   protected override void Attack()
    {
        base.Attack();
        GameObject spawnedHammerSaw = Instantiate(weaponData.Prefab);
        spawnedHammerSaw.transform.position = transform.position;
        //Assign the position to be the same as this object which is parent tho the player 

        spawnedHammerSaw.GetComponent<hammerSawBehaviour>().DirectionChecker(pm.lastMovedVector); // Reference and set the drection


    }
}
