using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    //InventoryManager inventory;
    //void Start()
    //{
    //    inventory = FindObjectOfType<InventoryManager>();

    //}

    //// Update is called once per frame
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if(col.gameObject.CompareTag("Player"))
    //    {
    //        OpenTreasureChest();
    //        Destroy(gameObject);
    //    }
    //}

    //public void OpenTreasureChest()
    //{
    //    if(inventory.GetPossibleEvolutions().Count <= 0)
    //    {
    //        Debug.Log("No possible evolutions");
    //        return;
    //    }

    //    WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];
    //    inventory.EvolveWeapon(toEvolve);
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerInventory p = col.GetComponent<PlayerInventory>();
        if (p)
        {
            bool randomBool = Random.Range(0, 2) == 0;

            OpenTreasureChest(p, randomBool);

            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest(PlayerInventory inventory, bool isHigherTier)
    {
        // Loop through every weapon to check whether it can evolve.
        foreach (PlayerInventory.Slot s in inventory.weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w.data.evolutionData == null) continue; // Ignore weapon if it cannot evolve.

            // Loop through every possible evolution of the weapon.
            foreach (ItemData.Evolution e in w.data.evolutionData)
            {
                // Only attempt to evolve weapons via treasure chest evolution.
                if (e.condition == ItemData.Evolution.Condition.treasureChest)
                {
                    bool attempt = w.AttemptEvolution(e, 0);
                    if (attempt) return; // If evolution succeeds, stop.
                }
            }
        }
    }

}
