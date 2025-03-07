using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventory;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        if(inventory.GetPossibleEvolutions().Count <= 0)
        {
            Debug.Log("No possible evolutions");
            return;
        }

        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
