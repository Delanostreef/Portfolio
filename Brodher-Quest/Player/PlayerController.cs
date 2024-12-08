using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Dead
}



public class PlayerController : MonoBehaviour
{

    //
    //  Editor settings
    //


    [Header("Controls")]
    [SerializeField] private float m_deadzone;

    [Header("Speed")]
    [SerializeField] private float m_maxSpeed;

    [SerializeField] private float m_walkAcceleration;
    [SerializeField] private float m_walkDeceleration;
    [SerializeField] private float m_iceAcceletarion;
    [SerializeField] private float m_iceDeceleration;

    [Header("Jump Settings")]
    [SerializeField] private float m_jumpStrength;
    [SerializeField] private float m_fallSpeed;

    [Header("Misc")]
    [SerializeField] private float m_groundedCheckSize;
    [SerializeField] private float m_wallCheckSize;
    [SerializeField] private LayerMask m_colliderLayer;

    [Header("DamageAnim")]
    [SerializeField] private float damageAnimLenght;

    [Header("Prefabs")]
    [SerializeField] private GameObject jumpCloud;
    [SerializeField] private GameObject landCloud;
    [SerializeField] private GameObject graveStone;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip damageSound;


    //
    //  Class Variables
    //


    private Vector2 movementVector = new Vector2();
    private PlayerState m_state;
    private bool isInAir = true;
    private bool frozen = false;

    //  Jump Timeout
    private float lastJumpMushroom = 0;

    private Vector3 startPos;
    private Checkpoint lastCheckpoint;

    //  Other components

    private Rigidbody2D rb;
    private new CapsuleCollider2D collider;
    private Animator animator;
    private AudioSource source;


    //
    //  Unity Events
    //


    private void Start()
    {
        startPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        state = PlayerState.Idle;
    }


    private void FixedUpdate()
    {

        //  Checking if dead

        if (state == PlayerState.Dead || frozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //  Checking if colliding with the ground

        RaycastHit2D grounded = IsGrounded();

        //  Updating velocity

        float xVel = UpdateXVel(grounded.collider);
        float yVel = UpdateYVel(grounded);

        rb.velocity = new Vector2(xVel, yVel);

        //  Animations

        if (!grounded)
            state = PlayerState.Jumping;

        else if (Math.Abs(movementVector.x) < m_deadzone)
            state = PlayerState.Idle;

        else state = PlayerState.Running;

    }


    //  Update functions


    private float UpdateXVel( Collider2D groundCollider )
    {
        //  Checking if touching a wall

        Vector2 pos = transform.position + Vector3.Scale(new Vector3(collider.offset.x + (m_wallCheckSize + collider.size.x) * .5f, collider.offset.y, 0), transform.localScale);
        Vector2 collision = new Vector2(m_wallCheckSize, collider.size.y * .7f);

        RaycastHit2D hit = Physics2D.BoxCast(pos, collision, 0, Vector2.zero, 0, m_colliderLayer);

        //  Getting acceleration and deceleration
        float acceleration = GetAcceleration(groundCollider);
        float deceleration = GetDeceleration(groundCollider);

        //  Velocity

        if (hit) return 0;
        
        float xVel = rb.velocity.x;

        //  Checking movement

        if (movementVector != Vector2.zero)
            xVel += acceleration * direction;

        else if (Mathf.Abs(xVel) - deceleration < 0)
            return 0;

        else
            xVel -= deceleration * Mathf.Sign(xVel);

        //  Checking if the speed is greater than the max speed

        if (Mathf.Abs(xVel) > m_maxSpeed) xVel = m_maxSpeed * Mathf.Sign(xVel);

        return xVel;
    }


    private float UpdateYVel( RaycastHit2D grounded )
    {
        float yVel = rb.velocity.y;

        //  Getting mushroom bounce

        if (grounded && grounded.collider.CompareTag("Mushroom") && lastJumpMushroom < Time.time)
        {
            lastJumpMushroom = Time.time + .05f;
            yVel = grounded.collider.GetComponent<Mushroom>().Bounce();
        }

        else if (yVel < .3 && !grounded) rb.AddForce(Vector3.down * m_fallSpeed);

        //  Landing cloud

        if (grounded && isInAir)
            Instantiate(landCloud).transform.position = transform.position;

        isInAir = !grounded;

        return yVel;
    }


    //
    //  Input System
    //


    private void OnMove( InputValue input )
    {
        movementVector = input.Get<Vector2>();

        if (Mathf.Abs(movementVector.x) < m_deadzone)
            movementVector = Vector2.zero;

        else direction = movementVector.x;
    }


    private void OnJump()
    {
        if (state == PlayerState.Dead) return;

        if (IsGrounded())
        {
            source.PlayOneShot(jumpSound);
            Instantiate(jumpCloud).transform.position = transform.position;
            rb.AddForce(new Vector2(0, m_jumpStrength));
        }
	}


    private void OnMenu()
	{
        SceneManager.LoadScene(0);
	}


    //
    //  Damage functions
    //


    public void Damage()
	{
        if (state == PlayerState.Dead) return;

        PlayerPrefs.SetInt("DeathCount", PlayerPrefs.GetInt("DeathCount", 1) + 1);
        source.PlayOneShot(damageSound);
        StartCoroutine(DamageEnumerator());
	}

	public void Restart()
	{
        transform.position = startPos;
	}


	//
	//  Gizmos
	//


	private void OnDrawGizmos()
	{
        if (!collider) return;

        //  Ugly math for wall gizmo
        //  Gets drawn from the direction side of the player

        Vector2 wall_pos = transform.position + Vector3.Scale(
            new Vector3(collider.offset.x + (m_wallCheckSize + collider.size.x) * .5f, collider.offset.y),
            transform.localScale
        );
        
        Vector2 wall_collision = new Vector2(m_wallCheckSize, collider.size.y *.7f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wall_pos, wall_collision);

        
        //  More ugly math but for jump gizmo
        //  Gets drawn from the player center to x units down

        Vector2 groundCheck_pos = transform.position + Vector3.Scale(
            new Vector2(collider.offset.x, m_groundedCheckSize *-.5f),
            transform.localScale
        );
        
        Vector2 groundCheck_collision = new Vector2(collider.size.x * .9f, m_groundedCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck_pos, groundCheck_collision);

    }


    //
    //  Coroutines
    //


    private IEnumerator DamageEnumerator()
	{
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        state = PlayerState.Dead;

        yield return new WaitForSeconds(damageAnimLenght);

        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = PlayerState.Idle;

        Instantiate(graveStone).transform.position = transform.position;

        if (lastCheckpoint)
        {
            lastCheckpoint.state = Checkpoint.States.Kamikazi;
            transform.position = lastCheckpoint.targetPos;
            lastCheckpoint = null;
        }

        else GameManager.instance.RestartLevel();

    }


    //
    //  Getters and Setters
    //


    private float direction
	{
        get => transform.localScale.x;
        set => transform.localScale = new Vector2(Mathf.Sign(value), 1);
	}


    public RaycastHit2D IsGrounded()
    {
        Vector2 pos = transform.position + Vector3.Scale(new Vector3(collider.offset.x, m_groundedCheckSize * -.5f, 0), transform.localScale);
        Vector2 collision = new Vector2(collider.size.x * .9f, m_groundedCheckSize);
        return Physics2D.BoxCast(pos, collision, 0, Vector2.zero, 0, m_colliderLayer);
    }



    public PlayerState state
	{
        get => m_state;
        set
		{
            if (m_state == value) return;

            animator.SetInteger("playerState", (int)value);
            m_state = value;
		}
	}


    public void SetCheckpoint( Checkpoint checkpoint )
    {
        //  Unloading checkpoint

        if (lastCheckpoint) lastCheckpoint.state = Checkpoint.States.Unused;

        //  Setting checkpoint

        lastCheckpoint = checkpoint;
        checkpoint.state = Checkpoint.States.Active;
    }


    //  Acceleration


    public float GetAcceleration( Collider2D collider )
	{
        if(collider) return collider.CompareTag("Ice") ? m_iceAcceletarion : m_walkAcceleration;
        return m_walkAcceleration;
    }


    public float GetDeceleration( Collider2D collider )
    {
        if (collider) return collider.CompareTag("Ice") ? m_iceDeceleration : m_walkDeceleration;
        return m_walkDeceleration;
    }


    public void SetFrozen(bool frozen) => this.frozen = frozen;
    public bool GetFrozen() => frozen;
}
