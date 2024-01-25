using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public LevelController levelController;
    public Animator animator;
    private Rigidbody2D _rb;
    private int _health;
    private bool _isPlayerImmune;
    private bool _isStunned;
    public float moveSpeed;
    private float _currentMoveSpeed;

    private void Start()
    {
        _isPlayerImmune = false;
        _rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        _health = 30;
        _currentMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (_health > 0 && !_isStunned)
        {
            levelController.canOpenDoor = false;

            if(_currentMoveSpeed <=moveSpeed)
            {
                _currentMoveSpeed += Time.deltaTime;
            }

            switch (player.transform.position.x - transform.position.x)
            {
                case < 0:
                    var positionXLeft = transform.position.x;
                    positionXLeft -= Time.deltaTime * _currentMoveSpeed;
                    transform.position = new Vector2(positionXLeft, transform.position.y);
                    break;
                case > 0:
                    var positionX = transform.position.x;
                    positionX += Time.deltaTime * _currentMoveSpeed;
                    transform.position = new Vector2(positionX, transform.position.y);
                    break;
            }
        }
    }

    public void GetDamage()
    {
        CameraShaker.Instance.ShakeOnce(3f, 1.25f, 0.75f, 0.75f);
        StartCoroutine(PlayerImmunity());
        _health -= 10;
        if (_health <= 0)
        {
            player.GetComponent<PlayerController>().PlaySfx("dieToEnemy");
            Die();
        }
        else
        {
            player.GetComponent<PlayerController>().PlaySfx("hitToEnemy");
            switch (player.transform.position.x - transform.position.x)
            {
                case < 0:
                    _rb.AddForce(Vector2.right * 4, ForceMode2D.Impulse);
                    animator.SetBool("RightHit", true);
                    break;
                case > 0:
                    _rb.AddForce(Vector2.left * 4, ForceMode2D.Impulse);
                    animator.SetBool("LeftHit", true);
                    break;
                default:
                    _rb.AddForce(Vector2.right * 4, ForceMode2D.Impulse);
                    animator.SetBool("RightHit", true);
                    break;
            }

            StartCoroutine(GettingDamage());
        }
    }

    private IEnumerator GettingDamage()
    {
        yield return new WaitForSeconds(1f);
        _rb.velocity = Vector2.zero;
        animator.SetBool("RightHit", false);
        animator.SetBool("LeftHit", false);
    }

    private void Die()
    {
        CameraShaker.Instance.ShakeOnce(4f, 2f, 1f, 1f);
        switch (player.transform.position.x - transform.position.x)
        {
            case < 0:
                animator.SetBool("Left", true);
                break;
            case > 0:
                animator.SetBool("Right", true);
                break;
            default:
                animator.SetBool("Left", true);
                break;
        }
        StartCoroutine(GetDestroyed());
        levelController.canOpenDoor = true;
    }

    private IEnumerator GetDestroyed()
    {
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(0.3f);
        Destroy(transform.gameObject);
    }

    private IEnumerator PlayerImmunity()
    {
        _isStunned = true;
        _isPlayerImmune = true;
        yield return new WaitForSeconds(1f);
        _isPlayerImmune = false;
        _isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            switch (player.GetComponent<PlayerController>().isOnDash)
            {
                case true:
                    GetDamage();
                    break;
                case false:
                    if(!_isPlayerImmune)
                    {
                        player.GetComponent<PlayerController>().GetKilled();
                    }
                    break;
            }
        }
    }
}
