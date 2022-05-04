using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMusic : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip soundClip;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            soundSource.clip = soundClip;
            soundSource.loop = true;
            soundSource.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
