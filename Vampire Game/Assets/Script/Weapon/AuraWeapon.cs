using UnityEngine;

public class AuraWeapon : Weapon
{
    protected Aura currentAura;

    // Update is called once per frame
    protected override void Update() { }

    public override void OnEquip()
    {
        // Try to replace the aura the weapon has with a new one.
        if (currentStats.auraPrefeab)
        {
            if (currentAura) Destroy(currentAura);
            currentAura = Instantiate(currentStats.auraPrefeab, transform);
            currentAura.weapon = this;
            currentAura.owner = owner;
        }
    }

    public override void OnUnequip()
    {
        if (currentAura) Destroy(currentAura);
    }

    public override bool DoLevelUp()
    {
        if (!base.DoLevelUp()) return false;

        // If there is an aura attached to this weapon, we update the aura.
        if (currentAura)
        {
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }
        return true;
    }

}
