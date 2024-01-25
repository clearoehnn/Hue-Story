using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    public SceneManager sceneManager;
    private bool _ifStartEnabled;

    public GameObject informationTab;
    public GameObject mainMenuItems;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Start()
    {
        _ifStartEnabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_ifStartEnabled)
            {
                case true:
                    sceneManager.ChangeScene(2);
                    break;
                
                case false:
                    EnableStart();
                    break;
            }
        }
    }

    private void EnableStart()
    {
        _ifStartEnabled = true;
        informationTab.SetActive(false);
        mainMenuItems.SetActive(true);
    }
}
