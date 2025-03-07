using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentprojectileSpeed;
    float currentMagnet;

    #region Current Stats Propreties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentprojectileSpeed; }
        set
        {
            if (currentprojectileSpeed != value)
            {
                currentprojectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentprojectileSpeed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
            }
        }
    }
    #endregion

    public ParticleSystem damageEffect;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTime;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMPro.TMP_Text levelText;


    
    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
    

    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move speed: " + currentMoveSpeed;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentprojectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
    
        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        if (invincibilityTime > 0)
        {
            invincibilityTime -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelupChecker();

        UpdateExpBar();
    }

    void LevelupChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges) 
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LV " + level.ToString();
    }

 
    
    void UpdateHealthBar()
    {
        //Update the health bar
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void TakeDamage(float dmg)
    {
        // if the player is not currently invincible, reduce health and start invincibility
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            if (damageEffect) Instantiate(damageEffect, transform.position, Quaternion.identity);


            invincibilityTime = invincibilityDuration;
            isInvincible = true;
            // if the player's health is less than 0, kill the player
            if (CurrentHealth <= 0)
            {
                Kill();
            }
            UpdateHealthBar();
        }
    }
    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponAndPassiveItemUI(inventory.weaponUISlots,inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += currentRecovery * Time.deltaTime;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {

        if(weaponIndex>= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {

        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
