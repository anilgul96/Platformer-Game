using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickup;
    [SerializeField] int pointToAdd = 100;

    private bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointToAdd);
            AudioSource.PlayClipAtPoint(coinPickup, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
