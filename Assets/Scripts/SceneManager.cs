using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    /*
     -----Scene ID's-----
     -Main Menu: 0
     -Level 1: 1
     -Level 2: 2
     -Level 3: 3
     -Level 4: 4
     -Level 5: 5
     --------------------
     */

    private Animator _animator;
    public AudioManager audioManager;
    private static readonly int LoadOut = Animator.StringToHash("LoadOut");
    private float _restartTimer;
    private bool _isRestarting;

    private void Start()
    {
        _restartTimer = 1;
        _animator = GetComponent<Animator>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySfx("sceneTransition");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isRestarting = true;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            _isRestarting = false;
            _restartTimer = 1;
        }

        if (_isRestarting)
        {
            _restartTimer -= Time.deltaTime;
            if (_restartTimer <= 0)
            {
                ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void ChangeScene(int sceneId)
    {
        StartCoroutine(SceneTransition(sceneId));
    }

    private IEnumerator SceneTransition(int sceneId)
    {
        audioManager.PlaySfx("sceneTransition");
        _animator.SetTrigger(LoadOut);
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneId);
    }
}
