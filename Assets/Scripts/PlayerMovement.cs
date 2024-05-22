using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedIncrement = 5f;
    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathkick = new Vector2(10f, 10f);
    [SerializeField] Transform gun;
    [SerializeField] GameObject bullet;
    Rigidbody2D playerRidigBody;
    Vector2 moveInput;
    Animator playerAnimator;
    CapsuleCollider2D checkPlayerIsTouching;
    BoxCollider2D playerFeetIsToucging;
    //LayerMask layerGround;
    float playerStartGravityScale;
    bool isAlive = true;


    void Start()
    {
        playerRidigBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        checkPlayerIsTouching = GetComponent<CapsuleCollider2D>();
        playerFeetIsToucging = GetComponent<BoxCollider2D>();
        playerStartGravityScale = playerRidigBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        CheckFire();
        Die();
    }

    void CheckFire()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("onAttack", false);
            return;
        }
        Instantiate(bullet, gun.position, transform.rotation);
        playerAnimator.SetBool("onAttack", true);
    }

    // BU FONKSİYON DOĞRU ÇALIŞMIYOR !!!
    // TETİKLENDİKTEN SONRA FALSE OLMUYOR !!!
    // void OnFire(InputValue value) 
    // {
    //     if (!isAlive) { return; }

    //     if (!value.isPressed)
    //     {
    //         playerAnimator.SetBool("isIdling", true);
    //         return;
    //     }

    //     playerAnimator.SetTrigger("Attack");
    //     Instantiate(bullet, gun.position, transform.rotation);
    // }


    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && (playerFeetIsToucging.IsTouchingLayers(LayerMask.GetMask("Ground"))))
        {
            playerRidigBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }


    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speedIncrement, playerRidigBody.velocity.y);
        playerRidigBody.velocity = playerVelocity;
        // yatayda hareket varsa, koşu animasyonunu çalıştır.
        // math.epsilon parametresi 0'a çok yakın ama sıfırdan birazcık büyük bir sayıyı tutar. Örn: 0.000001 gibi.
        bool playerHasHorizontalMovement = Mathf.Abs(playerRidigBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalMovement);
    }

    void FlipSprite()
    {
        // yataydaki x hızımızda - veya + bir hız varsa;
        bool playerHasHorizontalMovement = Mathf.Abs(playerRidigBody.velocity.x) > Mathf.Epsilon;

        // sıfırdan büyükse 1 yap(yani sağa hareket ettir), sıfırdan küçükse -1 yap(yani sola hareket ettir);
        if (playerHasHorizontalMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRidigBody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!checkPlayerIsTouching.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            playerRidigBody.gravityScale = playerStartGravityScale;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        playerRidigBody.gravityScale = 0;

        Vector2 playerVelocityVertical = new Vector2(playerRidigBody.velocity.x, moveInput.y * climbSpeed);
        playerRidigBody.velocity = playerVelocityVertical;

        bool playerHasVerticalMovement = Mathf.Abs(playerRidigBody.velocity.y) > Mathf.Epsilon;

        playerAnimator.SetBool("isClimbing", playerHasVerticalMovement);
    }

    void Die()
    {
        if (playerRidigBody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            playerRidigBody.gravityScale = 1f;
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRidigBody.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
