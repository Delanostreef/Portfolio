using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Mushroom : MonoBehaviour
{
	public enum State
	{
		Walk, Bounce
	}


	[SerializeField] private State m_state;

	[Header("Options")]
	[SerializeField] private float m_speed;
	[SerializeField] private float m_playerBoost;
	[SerializeField] private float m_bounceDuration;

	[Header("Check")]
	[SerializeField] private float m_wallCheckSize;
	[SerializeField] private float m_groundedCheckSize;
	[SerializeField] private LayerMask m_layerCheck;


	private new BoxCollider2D collider;
	private new Rigidbody2D rigidbody;
	private Animator animator;



	private void Start()
	{
		m_state = State.Walk;
		collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}



	private void FixedUpdate()
	{
		bool grounded = IsGrounded();

		//	Wall check

		Vector2 wall_pos = transform.position + Vector3.Scale(new Vector3((collider.offset.x + (m_wallCheckSize + collider.size.x) * .5f), collider.offset.y, 0), transform.localScale);
		Vector2 wall_collision = new Vector2(m_wallCheckSize, collider.size.y * .5f);

		RaycastHit2D hit = Physics2D.BoxCast(wall_pos, wall_collision, 0, Vector2.zero, 0, m_layerCheck);

		//	Checking if it hit the player
		if (hit && hit.collider.CompareTag("Player"))
			hit.collider.GetComponent<PlayerController>().Damage();


		//	Checking if it should turn
		else if(hit) direction *=-1;


		//	Applying speed

		if (grounded)
			rigidbody.velocity = new Vector2(m_speed * direction, rigidbody.velocity.y);

		else
			rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
	}



	private void OnDrawGizmos()
	{

		if (!collider) return;

		//  Ugly math but for ground detection

		Vector2 wall_pos = transform.position + Vector3.Scale(new Vector3((collider.offset.x + (m_wallCheckSize + collider.size.x) * .5f), collider.offset.y, 0), transform.localScale);
		Vector2 wall_collision = new Vector2(m_wallCheckSize, collider.size.y * .5f);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(wall_pos, wall_collision);

		//  More ugly math but for jump gizmo

		Vector2 jump_pos = transform.position + Vector3.Scale(new Vector3(collider.offset.x, m_groundedCheckSize * -.5f, 0), transform.localScale);
		Vector2 jump_collision = new Vector2(collider.size.x * .9f, m_groundedCheckSize);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(jump_pos, jump_collision);

	}

	//
	//	Coroutines
	//

	private IEnumerator BounceCoroutine()
	{
		state = State.Bounce;

		yield return new WaitForSeconds(m_bounceDuration);

		state = State.Walk;
	}


	//
	//	Getters and Setters
	//

	public float direction { get => transform.localScale.x; set => transform.localScale = new Vector2(value, transform.localScale.y); }


	public State state
	{
		get => m_state;
		protected set
		{
			m_state = value;
			animator.SetInteger("state", (int)value);
		}
	}


	public bool IsGrounded()
	{
		Vector2 pos = transform.position + Vector3.Scale(new Vector3(collider.offset.x, m_groundedCheckSize * -.5f, 0), transform.localScale);
		Vector2 collision = new Vector2(collider.size.x * .9f, m_groundedCheckSize);

		return Physics2D.BoxCast(pos, collision, 0, Vector2.zero, m_layerCheck);
	}


	public float Bounce()
	{
		StartCoroutine(BounceCoroutine());

		return m_playerBoost;
	}
}
