using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //Warning check to see if there is another singleton of this kind in the game
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    //Store current state of the game
    public GameState currentState;
    //Store previous state of the game
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;


    [Header("Current Stat Displays")]
    //Current stat displays;
    public TMPro.TMP_Text currentHealthDisplay;
    public TMPro.TMP_Text currentRecoveryDisplay;
    public TMPro.TMP_Text currentMoveSpeedDisplay;
    public TMPro.TMP_Text currentMightDisplay;
    public TMPro.TMP_Text currentProjectileSpeedDisplay;
    public TMPro.TMP_Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMPro.TMP_Text chosenCharacterName;
    public TMPro.TMP_Text levelReachedDisplay;
    public TMPro.TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; //The time limit in seconds
    float stopwatchTime; //The current time elapsed since the stopwatch started
    public TMPro.TMP_Text stopwatchDisplay;


    //flag check if the game is over
    public bool isGameOver = false;

    //Flag to check if the player os choosing their upgrades
    public bool choosingUpgrade;

    //Reference to the player's game object
    public GameObject playerObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA" + this + "DELETED");
        }
        DisableScreens();
    }

    private void Update()
    {
        //Define the behaviour for each state

        switch (currentState)
        {
            case GameState.Gameplay:
                //Code for the game play state
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                //Code for the game paused state
                break;

            case GameState.GameOver:
                //Code for the game over state
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME OVER");
                    DisplayResults();
                }
                break;

            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; //Pause the game for now
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);

                }
                break;

            default:
                Debug.Log("STATE DOES NOT EXIST");
                break;
        }
    }

 
 IEnumerator  GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
{
    // Start generating the floating text.
    GameObject textObj = new GameObject("Damage Floating Text");
    RectTransform rect = textObj.AddComponent<RectTransform>();
    TextMeshProUGUI tmpPro = textObj.AddComponent<TextMeshProUGUI>();
    tmpPro.text = text;
    tmpPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
    tmpPro.verticalAlignment = VerticalAlignmentOptions.Middle;
    tmpPro.fontSize = textFontSize;
    if (textFont) tmpPro.font = textFont;
    rect.position = referenceCamera.WorldToScreenPoint(target.position);

    // Makes sure this is destroyed after the duration finishes.
    Destroy(textObj, duration);

    // Parent the generated text object to the canvas.
    textObj.transform.SetParent(instance.damageTextCanvas.transform);

    // Pan the text upwards and fade it away over time.
    WaitForEndOfFrame w = new WaitForEndOfFrame();

    float t = 0;
    float yOffset = 0;
        //while (t < duration)
        //{
        //    // Wait for a frame and update the time.
        //    yield return w;
        //    t += Time.deltaTime;

        //    // Fade the text to the right alpha value.
        //    tmpPro.color = new Color(tmpPro.color.r, tmpPro.color.g, tmpPro.color.b, 1 - t / duration);

        //    // Pan the text upwards.
        //    yOffset += speed * Time.deltaTime;
        //    rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
        //}

        while (t < duration)
        {
            // Wait for a frame and update the time.
            yield return w;
            t += Time.deltaTime;

            if (tmpPro == null || rect == null) yield break; // Dừng Coroutine nếu object đã bị hủy

            // Fade the text to the right alpha value.
            tmpPro.color = new Color(tmpPro.color.r, tmpPro.color.g, tmpPro.color.b, 1 - t / duration);

            // Pan the text upwards.
            yOffset += speed * Time.deltaTime;

            if (referenceCamera != null && target != null)
            {
                rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
            }
            else
            {
                yield break; // Dừng nếu referenceCamera hoặc target bị hủy
            }
        }


    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        // if the canvas is not set,  end the function so we don't 
        // generate any floating text.
        if (!instance.damageTextCanvas)
        {
            return;
        }

        // Find a relevant camera that we can use to convert the world
        // position to screen position.
        if (!instance.referenceCamera)
        {
            instance.referenceCamera = Camera.main;
        }

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {

        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }

    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    //Define the method to check for pause and resume input
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponAndPassiveItemUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemUI.Count)
        {
            Debug.Log("Chosen weapons and passive items data lists have different lengths");
            return;
        }
        //Assign chosen weapons data to chosenWeaponUI
        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            //Check that the sprite of the corresponding element in chosen WeaponsData is not null
            if (chosenWeaponUI[i].sprite)
            {
                //Enable the corresponding element in chosenWeaponUI and set its sprite to the corresponding sprite in chosenWeaponsData
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                //If the sprite is null, disable the corresponding element in chosenWeaponUI
                chosenWeaponUI[i].enabled = false;
            }
        }

        //Assign chosen weapons data to chosenPassiveUI
        for (int i = 0; i < chosenPassiveItemUI.Count; i++)
        {
            //Check that the sprite of the corresponding element in chosen PassiveItemsData is not null
            if (chosenPassiveItemUI[i].sprite)
            {
                //Enable the corresponding element in chosenPassiveItemUI and set its sprite to the corresponding sprite in chosenPassiveItemsData
                chosenPassiveItemUI[i].enabled = true;
                chosenPassiveItemUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                //If the sprite is null, disable the corresponding element in chosenPassiveItemUI
                chosenPassiveItemUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateStopwatchDisplay()
    {
        //Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        //Update the stopwatch text to display the elapsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
