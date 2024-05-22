using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{

    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] Sprite leftArrow;
    SpriteRenderer defaultRenderer;
    public Rigidbody2D bulletRigidBody;
    PlayerMovement apo;
    public float xSpeed;
    void Start()
    {
        defaultRenderer = GetComponent<SpriteRenderer>();
        bulletRigidBody = GetComponent<Rigidbody2D>();
        apo = FindAnyObjectByType<PlayerMovement>();
        xSpeed = apo.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletRigidBody.velocity = new Vector2(xSpeed, 0f);
        if (xSpeed < 0)
        {
            defaultRenderer.sprite = leftArrow;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goober")
        {
            Debug.Log("Çarptı");
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }


}
