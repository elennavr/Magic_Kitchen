﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour {

	public GameObject ingredient;

    private KitleManager[] kitles;
    private bool dragging = false;
    private float distance;
    private Vector3 startPosition;

    bool flag = false;

    void Start() {
        startPosition = transform.position;

        kitles = FindObjectsOfType<KitleManager>();
    }

    void Update() {
        if (flag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }

    void OnMouseDown() {
        if (SceneManager.GetActiveScene().name == "Level A") { 
            if (GameManager.instance.gameState == GameManager.gameStates.Playing && !flag)
            {
            
                flag = true;
                //dragging = true;
                distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                if (this.gameObject.GetComponent<SkeletonAnimation>())
                {
                    this.gameObject.GetComponent<SkeletonAnimation>().GetComponent<MeshRenderer>().sortingLayerName = "Draged";
                }
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Draged";
                }
            }else if (flag){
                flag = false;
                if (this.gameObject.GetComponent<SkeletonAnimation>())
                {
                    this.gameObject.GetComponent<SkeletonAnimation>().GetComponent<MeshRenderer>().sortingLayerName = "Ingredients";
                }
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredients";
                }


                for (int i = 0; i < kitles.Length; i++)
                {
                    if (transform.position.x < kitles[i].transform.position.x + 1.5 && transform.position.x > kitles[i].transform.position.x - 1.5 && transform.position.y > kitles[i].transform.position.y - 2 && transform.position.y < kitles[i].transform.position.y + 2)
                    {
                        kitles[i].AddingElement(ingredient);
                        break;
                    }
                }

                transform.position = startPosition;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level B")
        {
            if (GameManager.instance.gameState == GameManager.gameStates.Playing)
            {
                for (int i = 0; i < kitles.Length; i++)
                {
                    kitles[i].AddingElement(ingredient);
                    break;
                }

            }
        }
    }

}
