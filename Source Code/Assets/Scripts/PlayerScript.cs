using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int health = 0;
    [SerializeField] public int maxHealth = 100; 
    public UIManager ui;
    public bool dead = false;
    Vector3 startPos;

    void Awake()
    {
        health = maxHealth; 
        Debug.Log("Player Start: " + health);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ui = GameObject.FindWithTag("manager").GetComponent<UIManager>();
            startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }    
    }

    public void UpdateHealth(int mod)
    {
        health += mod;

        Debug.Log("Player Health: " + health);

        if (health <= 0)
        {
            Debug.Log("f in chat you ded");
            Respawn();
        }
    }

    public void Respawn()
    {
        dead = true;
        PlayerReset(); 
        Debug.Log("no more unalive");
    }

    public void PlayerReset() 
    {
        this.transform.position = startPos;
        health = maxHealth;
    }
}
