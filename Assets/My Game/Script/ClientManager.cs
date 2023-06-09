﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class ClientManager : MonoBehaviour
{

    public enum clientsType
    {
        GIRL,
        NERD,
        ZOMBIE
    }

    public clientsType clientType;
    public GameObject potion;
    [Range(0, 3)]
    public float nbProgressPoint;
    public AudioClip[] sfxHappy;
    public AudioClip[] sfxSad;
    [HideInInspector]
    public int index;
    public int successfulClients = 0;

    private SkeletonAnimation skeletonAnimation;
    private AudioSource _AS;

    void Awake()
    {
        ChoosePotion();
    }

    void Start()
    {
        // Retreive Components
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        _AS = GetComponent<AudioSource>();

        StartCoroutine(PlaySound());

    }

    public void AddingPotion(GameObject potionGiven) {

        if (potion.tag == potionGiven.tag)
        {
            PlayerStats.instance.Gold += 10;
            SpawnManager.instance.DeativateClient(index);
            Destroy(potionGiven);
            successfulClients++;
        }
        else
        {
            AngryAnimation();
        }
    }
    
    //Choose Potion
    public void ChoosePotion()
    {
        potion = MainPotionManager.instance.PotionTab[Random.Range(0, MainPotionManager.instance.PotionTab.Length - 1)].potion.gameObject;
        SpawnManager.instance.ClientPotion(index, potion);
    }

    IEnumerator PlaySound(){
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 25f));

            // Play Happy sound
            if (SpawnManager.instance.SpawnPointsTab[index].progress.value > SpawnManager.instance.SpawnPointsTab[index].progress.maxValue/2)
            {
                _AS.clip = sfxHappy[Random.Range(0, sfxHappy.Length)];
                _AS.pitch = Random.Range(0.7f, 1.5f);
            }
            else
            {
                _AS.clip = sfxSad[Random.Range(0, sfxSad.Length)];
                _AS.pitch = Random.Range(0.7f, 1.1f);
                AngryAnimation();
            }

            if (GameManager.instance.gameState == GameManager.gameStates.Playing)
            {
                _AS.Play();
            }
        }
    }

    public void AngryAnimation()
    {

        switch (clientType)
        {
            case clientsType.ZOMBIE:
                skeletonAnimation.state.SetAnimation(0, "angry", false);
                skeletonAnimation.state.AddAnimation(0, "talk", true, 0f);
                break;
		
            case clientsType.GIRL:
                skeletonAnimation.state.SetAnimation(0, "tristemagicalgirl", false);
                skeletonAnimation.state.AddAnimation(0, "parlemagicalgirl", true, 0f);
                break;

            case clientsType.NERD:
                skeletonAnimation.state.SetAnimation(0, "nerdangry", false);
                skeletonAnimation.state.AddAnimation(0, "nerd1", true, 0f);
                break;

            default:
                break;
        }
    }
}
