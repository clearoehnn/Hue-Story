using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchGame : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LaunchMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
