using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text healthText;
    public Text timer;
    public Text debugText;
    public Text spellText;
    public Text finalScore;
    public Text finalScoreDeath;
    public Text spellPrompt;
    public ScoreManager sManager;
    public PlayerScript player;
    public WaveSpawner spawn;

    public GestureRecognitionScript checkClass;
    public GameObject playerRig;
    


    public GameObject pause;
    public GameObject levelTransition;
    public GameObject deathTransition;
    public bool paused = false;
    
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    bool pressed;



    private void Update() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            scoreText.text = "Score: " + sManager.playerScore;
            healthText.text = "Health: " + player.health;
            timer.text = "Time Left: " + spawn.timer.ToString("0.00");
            spellText.text = "Spell Type: " + checkClass.spellCheck;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            spellPrompt.text = "Spell Type: " + checkClass.spellCheck;
        }
        

        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool pressed);

        if (pressed) 
        {
            paused = !paused;
            pause.transform.position = new Vector3(playerRig.transform.position.x, playerRig.transform.position.y, playerRig.transform.position.z + 4);
            pause.SetActive(paused);
        }

        if (paused)
        {
            Time.timeScale = 0;
        }

        if (spawn.transition)
        {
            Time.timeScale = 0;
            finalScore.text = "Score: " + sManager.playerScore;
            levelTransition.transform.position = new Vector3(playerRig.transform.position.x, playerRig.transform.position.y, playerRig.transform.position.z + 4);
            levelTransition.SetActive(true);
        }

        if(player.dead)
        {
            Time.timeScale = 0;
            spawn.ClearEnemies();
            finalScoreDeath.text = "Score: " + sManager.playerScore;
            deathTransition.transform.position = new Vector3(playerRig.transform.position.x, playerRig.transform.position.y, playerRig.transform.position.z + 4);
            deathTransition.SetActive(true);
        }

    }
  
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene(3);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) GameReset();
        SceneManager.LoadScene(0);  
    }

    public void Continue()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
        
    }

    public void NextLevel()
    {
        spawn.level++;
        spawn.timer = spawn.waveTime[spawn.level];
        player.PlayerReset();

        spawn.transition = false;
        Time.timeScale = 1;
        levelTransition.SetActive(false);
    }

    void GameReset()
    {
        spawn.level = 1;
        spawn.timer = spawn.waveTime[spawn.level];
        spawn.transition = false;
        Time.timeScale = 1;
        levelTransition.SetActive(false);
        pause.SetActive(false);
        player.health = player.maxHealth;
        player.dead = false;
    }
}
