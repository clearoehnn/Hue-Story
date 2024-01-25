using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Assignments")]
    private Rigidbody2D _rb;
    private Animator _characterAnimator;
    public SceneManager sceneManager;
    public LevelController levelController;
    public GroundCheck groundCheck;
    private AudioSource _audioSource;
    [Header("Audio Clips")]
    public AudioClip dash;
    public AudioClip dieToEnemy;
    public AudioClip doubleJump;
    public AudioClip jump;
    public AudioClip fallToGround;
    public AudioClip hitToEnemy;
    public AudioClip fallToDeath;
    //public CameraShake cameraShake; (Old & custom camera shaker)
    [Header("Values")]
    public float jumpForce;
    private bool _doubleJumpEnabled;
    private int _jumpCount;
    private bool _isGrounded;
    
    public float walkSpeed;
    private bool _isMoveEnabled;
    private float _horizontalMove;
    
    private bool _isDashEnabled;
    private bool _isDashUsed;
    [HideInInspector]public bool isOnDash;
    public float dashForce;

    private int _collectibleCount;
    private bool _isDied;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _characterAnimator = transform.GetChild(0).GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        
        _doubleJumpEnabled = false;
        _isDashEnabled = false;
        _isMoveEnabled = true;
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex >= 6)
        {
            _doubleJumpEnabled = true;
        }
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex >= 10)
        {
            _isDashEnabled = true;
        }
    }

    
    void Update()
    {
        _isGrounded = groundCheck.isGrounded;
        _horizontalMove = Input.GetAxisRaw("Horizontal") * walkSpeed;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _horizontalMove != 0 && _isDashEnabled && _isMoveEnabled && !_isDashUsed)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if(_isMoveEnabled)
        {
            _rb.velocityX = _horizontalMove;
        }
        var velocityY = _rb.velocityY;
        var velocityX = _rb.velocityX;
        _rb.velocityY = Mathf.Clamp(velocityY, velocityY, 7);
        _rb.velocityX = Mathf.Clamp(velocityX, -10, 10);
    }

    public void PlaySfx(string audioClipName)
    {
        switch (audioClipName)
        {
            case "dash":
                _audioSource.clip = dash;
                break;
            case "dieToEnemy":
                _audioSource.clip = dieToEnemy;
                break;
            case "doubleJump":
                _audioSource.clip = doubleJump;
                break;
            case "jump":
                _audioSource.clip = jump;
                break;
            case "fallToGround":
                _audioSource.clip = fallToGround;
                break;
            case "hitToEnemy":
                _audioSource.clip = hitToEnemy;
                break;
            case "fallToDeath":
                _audioSource.clip = fallToDeath;
                break;
        }
        _audioSource.Play();
    }

    private IEnumerator Dash()
    {
        PlaySfx("dash");
        CameraShaker.Instance.ShakeOnce(2f, 1f, 0.5f, 0.5f);
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;
        _isMoveEnabled = false;
        _isDashUsed = true;
        
        switch (_horizontalMove)
        {
            case < 0:
                _rb.velocity = Vector2.zero;
                _rb.AddForce(Vector2.left * dashForce);
                isOnDash = true;
                break;
            case > 0:
                _rb.velocity = Vector2.zero;
                _rb.AddForce(Vector2.right * dashForce);
                isOnDash = true;
                break;
            default:
                break;
        }
        
        _characterAnimator.speed = 1;
        _characterAnimator.SetBool("Dash", true);
        _characterAnimator.SetBool("Jump", false);
        _characterAnimator.SetBool("DoubleJump", false);
        _characterAnimator.SetBool("Grounded", false);

        yield return new WaitForSeconds(0.2f);

        _characterAnimator.SetBool("Dash", false);
        _isMoveEnabled = true;
        isOnDash = false;
        _rb.gravityScale = 2;

        if (_isGrounded)
        {
            yield return new WaitForSeconds(0.5f);
            _isDashUsed = false;
        }
    }
    private void Jump()
    {
        if((_doubleJumpEnabled && _jumpCount < 2) | (_isGrounded))
        {
            if(_rb.velocityY <= 0)
            {
                _rb.velocityY = 0;
            }
            
            if(_jumpCount == 0)
            {
                switch (_isGrounded)
                {
                    case true:
                        PlaySfx("jump");
                        _characterAnimator.speed = 1;
                        _characterAnimator.SetBool("Grounded", false);
                        _characterAnimator.SetBool("DoubleJump", false);
                        _characterAnimator.SetBool("Dash", false);
                        _characterAnimator.SetBool("Jump", true);
                        _isDashUsed = false;
                        _rb.AddForce(Vector2.up * jumpForce);
                        break;
                    case false:
                        PlaySfx("doubleJump");
                        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.2f, 0.2f);
                        if (_horizontalMove > 0)
                        {
                            _characterAnimator.SetBool("Reversed", true);
                        }
                        else
                        {
                            _characterAnimator.SetBool("Reversed", false);
                        }
                        _characterAnimator.SetBool("Dash", false);
                        _characterAnimator.SetBool("Jump", false);
                        _characterAnimator.SetBool("Grounded", false);
                        _characterAnimator.SetBool("DoubleJump", true);
                        _rb.AddForce(Vector2.up * (jumpForce * 0.75f));
                        break;
                }
                {
                }
            }
            else
            {
                PlaySfx("doubleJump");
                CameraShaker.Instance.ShakeOnce(1f, 1f, 0.2f, 0.2f);
                if (_horizontalMove > 0)
                {
                    _characterAnimator.SetBool("Reversed", true);
                }
                else
                {
                    _characterAnimator.SetBool("Reversed", false);
                }
                _characterAnimator.SetBool("Dash", false);
                _characterAnimator.SetBool("Jump", false);
                _characterAnimator.SetBool("Grounded", false);
                _characterAnimator.SetBool("DoubleJump", true);
                _rb.AddForce(Vector2.up * (jumpForce * 0.75f));
            }
            
            if(_isGrounded)
            {
                _jumpCount++;
            }
            else
            {
                _jumpCount += 2;
            }
        }
    }

    public void Collectible()
    {
        _collectibleCount++;
        levelController.CollectibleCollected(_collectibleCount);
    }

    public void FallToGround()
    {
        PlaySfx("fallToGround");
        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.2f, 0.2f);
        _characterAnimator.SetBool("Grounded", true);
        _characterAnimator.SetBool("Dash", false);
        _characterAnimator.SetBool("Jump", false);
        _characterAnimator.SetBool("DoubleJump", false);
        _isDashUsed = false;
        _jumpCount = 0;
    }

    public void GetKilled()
    {
        _characterAnimator.SetBool("Die", true);
        PlaySfx("dieToEnemy");
        CameraShaker.Instance.ShakeOnce(4f, 1.5f, 1f, 1f);
        StartCoroutine(DieToEnemy());
    }

    private IEnumerator DieToEnemy()
    {
        _isMoveEnabled = false;
        _rb.velocityX -= Time.deltaTime * 3;
        _rb.velocityY -= Time.deltaTime * 3;
        if (_rb.velocityY < 0)
        {
            _rb.velocityY = 0;
        }
        if (_rb.velocityX < 0)
        {
            _rb.velocityX = 0;
        }
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;
        sceneManager.ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            sceneManager.ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        if (other.CompareTag("MainMenu"))
        {
            sceneManager.ChangeScene(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("DieTrigger"))
        {
            _characterAnimator.SetBool("Die", true);
            PlaySfx("fallToDeath");
            CameraShaker.Instance.ShakeOnce(4f, 1.5f, 1f, 1f);
            _isMoveEnabled = false;
            sceneManager.ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
