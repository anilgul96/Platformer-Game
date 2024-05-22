using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GooberMovement : MonoBehaviour
{
    [SerializeField] float gooberSpeed = 1f;
    Rigidbody2D gooberRigidBody;
    SpriteRenderer gooberFace;
    Animator gooberAnimator;

    CapsuleCollider2D isGooberTouchingLayer;

    BoxCollider2D goobersBoxCollider;

    LayerMask layerThatGooberTouch;



    void Start()
    {
        gooberFace = GetComponent<SpriteRenderer>();
        gooberRigidBody = GetComponent<Rigidbody2D>();
        goobersBoxCollider = GetComponent<BoxCollider2D>();
        gooberRigidBody.gravityScale = 5f;
    }

    void Update()
    {
        gooberRigidBody.velocity = new Vector2(gooberSpeed, 0f);
    }



    void OnTriggerExit2D(Collider2D other)
    {
        gooberSpeed = -gooberSpeed;
        FlipGooberFace();
    }

    void FlipGooberFace()
    {
        transform.localScale = new Vector2(Mathf.Sign(-gooberRigidBody.velocity.x), 1f);
    }

}
