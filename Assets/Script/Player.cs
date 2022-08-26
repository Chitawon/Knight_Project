using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [Header("Player Status")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float playerDamage;
    
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float accelerateMoveSpeed = 13f;
    [SerializeField] private float decelerationMoveSpeed = 16f;
    [SerializeField] private float accelPower = 0.96f;
    [SerializeField] private float stopPower = 1f;
    [SerializeField] private float turnPower = 1f;
    [SerializeField] private float frictionForce;
    [SerializeField] private float airDragForce;
    [SerializeField] [Range(0, 1f)] private float dropDownTime;
    [SerializeField] private float lastPressedJumpnDownTime;
    [SerializeField] private bool isClimbLadder;

    [Header("Player Jump")]
    [SerializeField] private float jumpSpeed = 12f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float fallGravityMult = 3f;
    [SerializeField] [Range(0, 0.5f)] private float coyoteTime;
    [SerializeField] private float lastOnGroundTime;
    [SerializeField] [Range(0, 0.5f)] private float jumpBufferTime; 
    [SerializeField] private float lastPressedJumpTime;
    [SerializeField] [Range(0, 1f)] private float jumpCutMultiplier; 

    [Header("Player Force")] 
    [SerializeField] private float playerKnockBackX =  30f;
    [SerializeField] private float playerKnockBackY =  4f;
    [SerializeField] private float playerBounceX =  30f;
    [SerializeField] private float playerBounceY =  4f;

    [Header("Player Atk Hitbox")]
    [SerializeField] private Transform atkFrontHitbox;
    [SerializeField] private Vector2 atkFrontRange;
    [SerializeField] private Transform atkDownHitbox;
    [SerializeField] private Vector2 atkDownRange;
    
    // Event
    public event EventHandler PlayerDeath;

    // State 
    [Header("Player State")]
    private bool isAlive = true;
    private bool isFacingRight = true;
    private bool isTakeHit;
    private bool isJumping;
    
    //Controller
    private float horizontalMove;
    private float verticalMove;
    private bool jumpUpMove;
    private bool jumpMove;

    // Cached component ref
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private CapsuleCollider2D playerBodyCollider2D;
    private CircleCollider2D playerFeetCollider2D;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask oneWayMask;
    [SerializeField] private LayerMask timeMask;
    [SerializeField] private LayerMask climbingMask;
    [SerializeField] private LayerMask enemiesMask;
    [SerializeField] private LayerMask throwMask;
    [SerializeField] private LayerMask spikeMask;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider2D = GetComponent<CapsuleCollider2D>();
        playerFeetCollider2D = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGravityScale(gravityScale);
        healthBar.SetMaxHealthBar(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        
        TimerCheck();
        InputCheck();
        GroundCheck();
        GravityCheck();
        ResetJump();
        Jump();
        JumpCut();
        Dropdown();
        FrontAttack();
        DownAttack();
    }
    
    void FixedUpdate()
    {
        DragFroce();
        Move();
        ClimbLadder();
    }

    private void TimerCheck()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        lastPressedJumpnDownTime -= Time.deltaTime;
    }
    
    private void InputCheck()
    {
        if (isTakeHit)
        {
            horizontalMove = 0f;
            verticalMove = 0f;
            ResetTakeHit();
            return;
        }
        
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        jumpMove = Input.GetButtonDown("Jump");
        jumpUpMove = Input.GetKeyUp(KeyCode.Space);
        
        if (horizontalMove != 0)
        {
            TurnSprite(horizontalMove > 0);
        }

        if (jumpMove)
        {
            lastPressedJumpTime = jumpBufferTime;
        }

        if (jumpMove && verticalMove < 0)
        {
            lastPressedJumpnDownTime = dropDownTime;
        }

        if (verticalMove > 0 || verticalMove < 0)
        {
            isClimbLadder = true;
        }
    }

    private void GroundCheck()
    {
        if (!playerFeetCollider2D.IsTouchingLayers(groundMask) 
            && !playerFeetCollider2D.IsTouchingLayers(oneWayMask) 
            && !playerFeetCollider2D.IsTouchingLayers(timeMask) 
            && !playerFeetCollider2D.IsTouchingLayers(climbingMask))
        {
            return;
        }
        
        lastOnGroundTime = coyoteTime;
    }
    
    private void GravityCheck()
    {
        if (playerRigidbody.velocity.y >= 0 && !CanClimbLadder())
        {
            SetGravityScale(gravityScale);
        }
        else if (CanClimbLadder())
        {
            SetGravityScale(0);
        }
        else
        {
            SetGravityScale(gravityScale * fallGravityMult);
        }
    }

    private void DragFroce()
    {
        float amount;
        if (lastOnGroundTime <= 0)
        {
            amount = airDragForce;
        }
        else
        {
            amount = frictionForce;
        }
        
        Vector2 force = amount * playerRigidbody.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(playerRigidbody.velocity.x), Mathf.Abs(force.x)); //ensures we only slow the player down, if the player is going really slowly we just apply a force stopping them
        force.y = Mathf.Min(Mathf.Abs(playerRigidbody.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(playerRigidbody.velocity.x); //finds direction to apply force
        force.y *= Mathf.Sign(playerRigidbody.velocity.y);

        playerRigidbody.AddForce(-force, ForceMode2D.Impulse); //applies force against movement direction
    }
    
    private void Move()
    {
        float targetSpeed = horizontalMove * moveSpeed;
        float speedDif = targetSpeed - playerRigidbody.velocity.x;
        
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerateMoveSpeed : decelerationMoveSpeed;
        
        //if we want to run but are already going faster than max run speed
        if (((playerRigidbody.velocity.x > targetSpeed && targetSpeed > 0.01f) || (playerRigidbody.velocity.x < targetSpeed && targetSpeed < -0.01f)))
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = stopPower;
        }
        else if (Mathf.Abs(playerRigidbody.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(playerRigidbody.velocity.x)))
        {
            velPower = turnPower;
        }
        else
        {
            velPower = accelPower;
        }
        
        // applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(playerRigidbody.velocity.x, movement, 1); // lerp so that we can prevent the Run from immediately slowing the player down, in some situations eg wall jump, dash 
        
        Vector2 playerVelocity = Vector2.right * movement;
        playerRigidbody.AddForce(playerVelocity);

        bool isMoving = Mathf.Abs(horizontalMove) > 0 && Mathf.Abs(verticalMove) == 0;
        playerAnimator.SetBool("Running", isMoving);
    }

    private void Jump()
    {
        if (lastPressedJumpTime > 0 && CanJump() && lastPressedJumpnDownTime < 0)
        {
            isJumping = true;
            isClimbLadder = false;
            lastOnGroundTime = 0;
            lastPressedJumpTime = 0;
            playerRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        playerAnimator.SetBool("Jumping", isJumping);
    }

    private void JumpCut()
    {
        if (CanJumpCut())
        {
            playerRigidbody.AddForce(Vector2.down * playerRigidbody.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }

    private void Dropdown()
    {
        if (CanDropDown())
        {
            Vector2 froce = Vector2.down * jumpSpeed;
            playerRigidbody.AddForce(froce, ForceMode2D.Impulse);
        }
    }

    private void ClimbLadder()
    {
        if (!CanClimbLadder())
        {
            isClimbLadder = false;
            return;
        }
        
        float targetSpeed = verticalMove * moveSpeed;
        float speedDif = targetSpeed - playerRigidbody.velocity.y;
        
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerateMoveSpeed : decelerationMoveSpeed;
        
        //if we want to run but are already going faster than max run speed
        if (((playerRigidbody.velocity.y > targetSpeed && targetSpeed > 0.01f) || (playerRigidbody.velocity.y < targetSpeed && targetSpeed < -0.01f)))
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = stopPower;
        }
        else if (Mathf.Abs(playerRigidbody.velocity.y) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(playerRigidbody.velocity.y)))
        {
            velPower = turnPower;
        }
        else
        {
            velPower = accelPower;
        }
        
        // applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(playerRigidbody.velocity.y, movement, 1); // lerp so that we can prevent the Run from immediately slowing the player down, in some situations eg wall jump, dash 
        
        Vector2 playerVelocity = Vector2.up * movement;
        playerRigidbody.AddForce(playerVelocity);
    }

    private void FrontAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Collider2D[] enemiesToDmg = Physics2D.OverlapBoxAll(atkFrontHitbox.position, atkFrontRange, 0, enemiesMask);
            for (int i = 0; i < enemiesToDmg.Length; i++)
            {
                enemiesToDmg[i].GetComponent<Enemy>().ProcessHit(playerDamage);
            }
        }
        playerAnimator.SetBool("Attack", Input.GetButtonDown("Fire1"));
    }

    private void DownAttack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Vector2 bounceForce = new Vector2(playerBounceX, playerBounceY);
            if (isFacingRight)
            {
                bounceForce.x *= 1;
            }
            else
            {
                bounceForce.x *= -1;
            }
            
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(atkDownHitbox.position, atkDownRange, 0, enemiesMask);
            if (hitEnemies.Length > 0)
            {
                for (int i = 0; i < hitEnemies.Length; i++)
                {
                    if (i <= 0)
                    {
                        playerRigidbody.AddForce(bounceForce, ForceMode2D.Impulse);
                    }
                    hitEnemies[i].GetComponent<Enemy>().ProcessHit(playerDamage);
                }
            }
        }
        playerAnimator.SetBool("Ground Attack", Input.GetButtonDown("Fire2"));
    }
    
    private void TurnSprite(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
            isFacingRight = !isFacingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isAlive && !isTakeHit)
        {
            if (1 << col.gameObject.layer == enemiesMask.value)
            {
                GetHit(col.gameObject.GetComponent<Enemy>().GetDamege(), col.gameObject.GetComponent<Enemy>().IsFacingRight(), col.gameObject.GetComponent<Enemy>().HaveKnockBack());
            }
            else if (1 << col.gameObject.layer == throwMask.value)
            {
                GetHit(col.gameObject.GetComponent<ThrowObj>().GetDamage(), col.gameObject.GetComponent<ThrowObj>().IsFacingRight(), true);
            }
            else if (1 << col.gameObject.layer == spikeMask.value)
            {
                GetHit(col.gameObject.GetComponent<Spinkground>().GetDamege(), !isFacingRight, true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isAlive && !isTakeHit)
        {
            if (1 << col.gameObject.layer == enemiesMask.value)
            {
                GetHit(col.gameObject.GetComponent<Enemy>().GetDamege(), col.gameObject.GetComponent<Enemy>().IsFacingRight(), col.gameObject.GetComponent<Enemy>().HaveKnockBack());
            }
        }
    }

    private void GetHit(float damege, bool forceFromRight, bool haveKnockBack)
    {
        if (damege <= 0)
        {
            return;
        }
        
        health -= damege;
        healthBar.SetHealthBar(health);
        isTakeHit = true;
        Vector2 hitForce = new Vector2(playerKnockBackX, playerKnockBackY);
        if (!forceFromRight)
        {
            hitForce.x *= -1;
        }

        if (haveKnockBack)
        {
            playerRigidbody.AddForce(hitForce, ForceMode2D.Impulse);
        }

        if (health < 0)
        {
            isAlive = false;
            PlayerDeath?.Invoke(this, EventArgs.Empty);
            playerAnimator.SetTrigger("Death");
        }
        
    }

    private void SetGravityScale(float gravityScale)
    {
        playerRigidbody.gravityScale = gravityScale;
    }

    private bool CanClimbLadder()
    {
        return isClimbLadder && (playerFeetCollider2D.IsTouchingLayers(climbingMask) || playerBodyCollider2D.IsTouchingLayers(climbingMask));
    }
    
    private bool CanDropDown()
    {
        return lastPressedJumpnDownTime > 0 && playerFeetCollider2D.IsTouchingLayers(oneWayMask);
    }
    
    private bool CanJump()
    {
        return !isJumping && lastOnGroundTime > 0;
    }
    
    private bool CanJumpCut()
    {
        return jumpUpMove && isJumping && playerRigidbody.velocity.y > 0;
    }

    private void ResetJump()
    {
        if (isJumping && playerRigidbody.velocity.y < 0)
        {
            isJumping = false;
        }
    }

    private void ResetTakeHit()
    {
        if (!playerFeetCollider2D.IsTouchingLayers(groundMask) && !playerFeetCollider2D.IsTouchingLayers(oneWayMask) && !playerFeetCollider2D.IsTouchingLayers(timeMask))
        {
            return;
        }
        
        isTakeHit = false;
    }
}