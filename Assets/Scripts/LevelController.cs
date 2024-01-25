using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int neededCollectibleCount;
    [HideInInspector] public int collectibleCount;

    public GameObject door;
    private Animator _doorAnimator;
    public GameObject collectiblesUi;
    public AudioManager audioManager;
    private GameObject _player;
    
    public Color collectibleUiColor = new Color(255, 150, 0, 255);
    public bool canOpenDoor;
    private bool _ableToOpenDoor;
    public GameObject doorOpenText;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        canOpenDoor = true;
        
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex <= 22)
        {
            _ableToOpenDoor = false;
        }
        else
        {
            _ableToOpenDoor = true;
        }
        _doorAnimator = door.GetComponent<Animator>();
        
    }
    
    void Update()
    {
        if ((collectibleCount == neededCollectibleCount) && canOpenDoor && _ableToOpenDoor &&
            (Vector3.Distance(_player.transform.position, door.transform.position) < 5))
        {
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex <= 22)
            {
                if(doorOpenText != null)
                {
                    doorOpenText.SetActive(true);
                }
            }
            
            if(Input.GetKeyDown(KeyCode.E))
            {
                LevelPassCleared();
            }
        }
        else
        {
            if(doorOpenText != null)
            {
                doorOpenText.SetActive(false);
            }
        }
    }

    private void LevelPassCleared()
    {
        audioManager.PlaySfx("doorOpen");
        _doorAnimator.SetBool("Open", true);
    }

    public void CollectibleCollected(int collectibleCountForUi)
    {
        audioManager.PlaySfx("collectCollectible");
       collectibleCount++;
        switch (collectibleCountForUi)
        {
            case 1:
                collectiblesUi.transform.GetChild(0).gameObject.GetComponent<Image>().color =
                    collectibleUiColor;
                break;
            case 2:
                collectiblesUi.transform.GetChild(1).gameObject.GetComponent<Image>().color =
                    collectibleUiColor;
                break;
            case 3:
                collectiblesUi.transform.GetChild(2).gameObject.GetComponent<Image>().color =
                    collectibleUiColor;
                _ableToOpenDoor = true;
                break;
        }
    }
}
