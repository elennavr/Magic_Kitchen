using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    // Static variable PlayerStats
    public static PlayerStats instance;

    //Public Variables
    public int lifePoints = 3;
    public string satisfiedClients;
    public string satPercentage;
    public string[] lifeStamps = new string[3];
    public string username;

    // Private Variables
    [SerializeField]
    private int _gold;

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }


    void Awake()
    {
        instance = this;
        
        username = PlayerPrefs.GetString("Username");
    }

    void Update()
    {
        if (Gold < 0)
        {
            Gold = 0;
        }
        UIManager.instance.goldText.text = "" + Gold;

        UIManager.instance.ModifyLife(lifePoints);
        if (lifePoints <= 0)
        {
            GameManager.instance.gameState = GameManager.gameStates.GameOver;
            timer.instance.lifeTimestamps(lifeStamps);
            satisfiedClients = (_gold / 10).ToString();
            satPercentage = (((_gold / 10) * 100D)/((_gold / 10) +3D)).ToString();
        }


    }
}