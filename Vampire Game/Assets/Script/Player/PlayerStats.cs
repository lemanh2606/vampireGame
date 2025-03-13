using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    float health;

    

 

    #region Current Stats Propreties
    public float CurrentHealth
    {
        get { return health; }

        // If we try and ser ==t the current health, the UI interface 
        // on the pause screen will also be updated.
        set
        {
            // check if the value has change
            if(health != value)
            {
                health = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format("Health: {0} / {1}", health, actualStats.maxHealth);
                }
            }
        }
    }


    public float MaxHealth
    {
        get { return actualStats.maxHealth; }

        // If we try and set the max health, the UI interface
        // on the pause screen will also be updated.
        set
        {
            // Check if the value has changed
            if (actualStats.maxHealth != value)
            {
                actualStats.maxHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                }
                // Update the real time value of the stat
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }
    public float CurrentRecovery
    {
        get { return Recovery; }
        set { Recovery = value; }
    }

    public float Recovery
    {
        get { return actualStats.recovery; }
        set
        {
            // Check if the value has changed
            if (actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return MoveSpeed; }
        set { MoveSpeed = value; }
    }
    public float MoveSpeed
    {
        get { return actualStats.moveSpeed; }
        set
        {
            // Check if the value has changed
            if (actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + actualStats.moveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return Might; }
        set { Might = value; }
    }

    public float Might
    {
        get { return actualStats.might; }
        set
        {
            // Check if the value has changed
            if (actualStats.might != value)
            {
                actualStats.might = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + actualStats.might;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed = value; }
    }

    public float Speed
    {
        get { return actualStats.speed; }
        set
        {
            // Check if the value has changed
            if (actualStats.speed != value)
            {
                actualStats.speed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return Magnet; }
        set { Magnet = value; }
    }

    public float Magnet
    {
        get { return actualStats.magnet; }
        set
        {
            // Check if the value has changed
            if (actualStats.magnet != value)
            {
                actualStats.magnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + actualStats.magnet;
                }
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

    PlayerInventory inventory;
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

        inventory = GetComponent<PlayerInventory>();

        // Assign the variable
         baseStats = actualStats = characterData.stats;
        health = actualStats.maxHealth;

    }

    void Start()
    {
        // Spawn the starting weapon
        inventory.Add(characterData.StartingWeapon);

        experienceCap = levelRanges[0].experienceCapIncrease;

        // Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + CurrentMagnet;

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

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }
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
        healthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        // if the player is not currently invincible, reduce health and start invincibility
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);


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
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }

    [System.Obsolete("Old funtion the is kept to maintain compatibility with InventoryManager. Will be removed soon.")]
    public void SpawnWeapon(GameObject weapon)
    {

        if(weaponIndex>= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        //inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    [System.Obsolete("No need spawn passive items direcly now.")]
    public void SpawnPassiveItem(GameObject passiveItem)
    {

        if (passiveItemIndex >= inventory.passiveSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        //inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
