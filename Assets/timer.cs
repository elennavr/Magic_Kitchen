using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public float timeValue = 0;
    public Text timerText;

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
        }
        
    }

    void DisplayTime(float timeToDisplay)
    {        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
    }
}