using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public static timer instance;

    public float timeValue = 0;
    public Text timerText;

    public string[] timeStamps = new string[3];
    public int life = 3;
    public bool[] first = {true, true};
    public bool game = true;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameState == GameManager.gameStates.Playing)
        {
            timeValue += Time.deltaTime;
            DisplayTime(timeValue);

            life = PlayerStats.instance.lifePoints;
            lifeTimestamps();
        }
        else
        {
            //For testing purposes
            while (game) { 
                Debug.Log(timeStamps[0] + " " + timeStamps[1] + " " + timeStamps[2]);
                game = false;
            }
        }
        
    }

    void DisplayTime(float timeToDisplay)
    {        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
    }

    public void lifeTimestamps()
    {
        if(life == 2 && first[0])
        {
            timeStamps[0] = timerText.text;
            first[0] = false;
        }
        else if(life == 1 && first[1])
        {
            timeStamps[1] = timerText.text;
            first[1] = false;
        }
        else
        {
            timeStamps[2] = timerText.text;
        }
    }
}