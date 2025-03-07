using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{

    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);



    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMPro.TMP_Text upgradeNameDisplay;
        public TMPro.TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>(); //List of upgrade options for weapons
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); //List of upgrade options for passive items
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); //List of UI for upgrade options present in the scene

    public List<WeaponEvolutionBlueprint> weaponEvolutions = new List<WeaponEvolutionBlueprint>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;//Enable the image component
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled |= true; //Enable the image component
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefabs)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefabs, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefabs)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefabs, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;

            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    //void ApplyUpgradeOptions()
    //{
    //    foreach (var upgradeOption in upgradeUIOptions)
    //    {
    //        int upgradeType = Random.Range(1, 3); //Choose between weapon and passive items
    //        if (upgradeType == 1)
    //        {
    //            WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];
    //            if (chosenWeaponUpgrade != null)
    //            {
    //                bool newWeapon = false;
    //                for (int i = 0; i < weaponSlots.Count; i++)
    //                {
    //                    if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
    //                    {
    //                        newWeapon = false;
    //                        if (!newWeapon)
    //                        {
    //                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i)); //Apply button functionally
    //                            //Set the description and name to be that of the next level
    //                            upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefabs.GetComponent<WeaponController>().weaponData.Description;
    //                            upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefabs.GetComponent<WeaponController>().weaponData.Name;
    //                        }
    //                        break;
    //                    }
    //                    else
    //                    {
    //                        newWeapon = true;
    //                    }
    //                }
    //                if (newWeapon)//Spawn a new weapon
    //                {
    //                    upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); //Apply button functionally
    //                                                                                                                                  //Apply initial description and name       
    //                    upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
    //                    upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
    //                }
    //                upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
    //            }
    //        }
    //        else if (upgradeType == 2)
    //        {
    //            PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[Random.Range(0, passiveItemUpgradeOptions.Count)];

    //            if (chosenPassiveItemUpgrade != null)
    //            {
    //                bool newPassiveItem = false;
    //                for (int i = 0; i < passiveItemSlots.Count; i++)
    //                {
    //                    if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
    //                    {
    //                        newPassiveItem = false;

    //                        if (!newPassiveItem)
    //                        {
    //                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i)); //Apply button functionally
    //                            //Set the description and name to be that of the next level
    //                            upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs.GetComponent<PassiveItem>().passiveItemData.Description;
    //                            upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs.GetComponent<PassiveItem>().passiveItemData.Name;
    //                        }
    //                        else
    //                        {
    //                            newPassiveItem = true;
    //                        }
    //                        break;
    //                    }
    //                }
    //                if (newPassiveItem)
    //                {
    //                    upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); //Apply button functionally
    //                    //Apply initial description and name       
    //                    upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
    //                    upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
    //                }
    //                upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
    //            }
    //        }
    //    }
    //}

    //this true 1:04:35
    //void ApplyUpgradeOptions()
    //{
    //    List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
    //    List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);
    //    foreach (var upgradeOption in upgradeUIOptions)
    //    {
    //        if(availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
    //        {
    //            return;
    //        }
    //        int upgradeType;

    //        if(availableWeaponUpgrades.Count == 0)
    //        {
    //            upgradeType = 2;
    //        }
    //        else if(availablePassiveItemUpgrades.Count == 1)
    //        {
    //            upgradeType = 1;
    //        }
    //        else
    //        {
    //            upgradeType = Random.Range(1, 3);
    //        }


    //        //int upgradeType = Random.Range(1, 3); // Chọn giữa vũ khí và vật phẩm thụ động
    //        if (upgradeType == 1)
    //        {
    //            WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

    //            availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

    //            if (chosenWeaponUpgrade != null)
    //            {
    //                bool newWeapon = true; // Mặc định giả sử là vũ khí mới

    //                for (int i = 0; i < weaponSlots.Count; i++)
    //                {
    //                    if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
    //                    {
    //                        newWeapon = false; // Đã tồn tại vũ khí này, không cần tạo mới
    //                        upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));

    //                        if (chosenWeaponUpgrade.weaponData.NextLevelPrefabs != null)
    //                        {
    //                            var nextLevelWeapon = chosenWeaponUpgrade.weaponData.NextLevelPrefabs.GetComponent<WeaponController>();
    //                            if (nextLevelWeapon != null)
    //                            {
    //                                upgradeOption.upgradeDescriptionDisplay.text = nextLevelWeapon.weaponData.Description;
    //                                upgradeOption.upgradeNameDisplay.text = nextLevelWeapon.weaponData.Name;
    //                            }
    //                        }
    //                        break;
    //                    }
    //                }

    //                if (newWeapon) // Nếu vũ khí chưa tồn tại, thêm mới
    //                {
    //                    upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
    //                    upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
    //                    upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
    //                }
    //                upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
    //            }
    //        }
    //        else if (upgradeType == 2)
    //        {
    //            PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
    //            availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

    //            if (chosenPassiveItemUpgrade != null)
    //            {
    //                bool newPassiveItem = true; // Mặc định giả sử là vật phẩm mới

    //                for (int i = 0; i < passiveItemSlots.Count; i++)
    //                {
    //                    if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
    //                    {
    //                        newPassiveItem = false; // Đã tồn tại vật phẩm này, không cần tạo mới
    //                        upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex));

    //                        if (chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs != null)
    //                        {
    //                            var nextLevelItem = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs.GetComponent<PassiveItem>();
    //                            if (nextLevelItem != null)
    //                            {
    //                                upgradeOption.upgradeDescriptionDisplay.text = nextLevelItem.passiveItemData.Description;
    //                                upgradeOption.upgradeNameDisplay.text = nextLevelItem.passiveItemData.Name;
    //                            }
    //                        }
    //                        break;
    //                    }
    //                }

    //                if (newPassiveItem) // Nếu vật phẩm chưa tồn tại, thêm mới
    //                {
    //                    upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
    //                    upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
    //                    upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
    //                }
    //                upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
    //            }
    //        }
    //    }
    //}

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);
        foreach (var upgradeOption in upgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }
            int upgradeType;

            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 1)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }


            //int upgradeType = Random.Range(1, 3); // Chọn giữa vũ khí và vật phẩm thụ động
            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = true; // Mặc định giả sử là vũ khí mới

                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false; // Đã tồn tại vũ khí này, không cần tạo mới
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));

                            if (chosenWeaponUpgrade.weaponData.NextLevelPrefabs != null)
                            {
                                var nextLevelWeapon = chosenWeaponUpgrade.weaponData.NextLevelPrefabs.GetComponent<WeaponController>();
                                if (nextLevelWeapon != null)
                                {
                                    upgradeOption.upgradeDescriptionDisplay.text = nextLevelWeapon.weaponData.Description;
                                    upgradeOption.upgradeNameDisplay.text = nextLevelWeapon.weaponData.Name;
                                }
                            }
                            else
                            {
                                DisableUpgradeUI(upgradeOption);
                            }
                            break;
                        }
                    }

                    if (newWeapon) // Nếu vũ khí chưa tồn tại, thêm mới
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newPassiveItem = true; // Mặc định giả sử là vật phẩm mới

                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false; // Đã tồn tại vật phẩm này, không cần tạo mới
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex));

                            if (chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs != null)
                            {
                                var nextLevelItem = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefabs.GetComponent<PassiveItem>();
                                if (nextLevelItem != null)
                                {
                                    upgradeOption.upgradeDescriptionDisplay.text = nextLevelItem.passiveItemData.Description;
                                    upgradeOption.upgradeNameDisplay.text = nextLevelItem.passiveItemData.Name;
                                }
                            }
                            else
                            {
                                DisableUpgradeUI(upgradeOption);
                            }
                            break;
                        }
                    }

                    if (newPassiveItem) // Nếu vật phẩm chưa tồn tại, thêm mới
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }


    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
    public List<WeaponEvolutionBlueprint> GetPossibleEvolutions()
    {
        List<WeaponEvolutionBlueprint> possibleEvolutions = new List<WeaponEvolutionBlueprint>();
        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon != null)
            {
                foreach (PassiveItem catalyst in passiveItemSlots)
                {
                    if (catalyst != null)
                    {
                        foreach (WeaponEvolutionBlueprint evolution in weaponEvolutions)
                        {
                            if (weapon.weaponData.Level >= evolution.baseWeaponData.Level && catalyst.passiveItemData.Level >= evolution.catalystPasiveItemData.Level)
                            {
                                possibleEvolutions.Add(evolution);
                            }
                        }
                    }
                }
            }

        }

        return possibleEvolutions;
    }


    public void EvolveWeapon(WeaponEvolutionBlueprint evolution)
    {
        for(int weaponSlotIndex = 0; weaponSlotIndex < weaponSlots.Count; weaponSlotIndex++)
        {
            WeaponController weapon = weaponSlots[weaponSlotIndex];

            if (!weapon)
            {
                continue;
            }
            for (int catalystSlotIndex = 0; catalystSlotIndex < passiveItemSlots.Count; catalystSlotIndex++)
            {
                PassiveItem catalyst = passiveItemSlots[catalystSlotIndex];

                if (!catalyst)
                {
                    continue;
                }

                if (weapon && catalyst && weapon.weaponData.Level >= evolution.baseWeaponData.Level && catalyst.passiveItemData.Level >= evolution.catalystPasiveItemData.Level)
                {
                    GameObject evolveWeapon = Instantiate(evolution.evolvedWeapon, transform.position, Quaternion.identity);

                    WeaponController evolvedWeaponController = evolveWeapon.GetComponent<WeaponController>();

                    evolveWeapon.transform.SetParent(transform);
                    AddWeapon(weaponSlotIndex, evolvedWeaponController);
                    Destroy(weapon.gameObject);

                    // Update level an icon

                    weaponLevels[weaponSlotIndex] = evolvedWeaponController.weaponData.Level;
                    weaponUISlots[weaponSlotIndex].sprite = evolvedWeaponController.weaponData.Icon;

                    // Update the update options
                    weaponUpgradeOptions.RemoveAt(evolvedWeaponController.weaponData.EvolveUpdateToRemove);


                    Debug.LogWarning("EVOLVED!");
                    return;
                }
            }
        }
        }

    }
