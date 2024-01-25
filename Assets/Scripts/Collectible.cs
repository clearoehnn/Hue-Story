using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            playerController.Collectible();
            Destroy(transform.parent.gameObject);
        }
    }
}
