using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public enum State
	{
		idle,
		pickedUp
	}

	
	//
	//	Editor variables
	//


	[Header("Options")]
	[SerializeField] private float pickupAnimLength;


	//
	//	Class Variables
	//


	private State m_state = State.idle;
	private Animator animator;


	[HideInInspector]
	public int listIndex;

	//
	//	Unity Events
	//


	private void Start()
	{
		animator = GetComponent<Animator>();
	}


	private void OnTriggerEnter2D( Collider2D collision )
	{
		if (state == State.pickedUp) return;
		if (!collision.CompareTag("Player")) return;

		CoinManager.instance.CoinCollected(this);
		StartCoroutine(Pickup());
	}


	//
	//	Coroutines
	//


	private IEnumerator Pickup()
	{
		state = State.pickedUp;
		transform.localPosition += Vector3.up * .25f;
		yield return new WaitForSeconds(pickupAnimLength);
		Destroy(gameObject);
	}


	//
	//	Getters and Setters
	//

	public State state
	{
		get => m_state;
		private set {
			m_state = value;
			animator.SetInteger("state", (int)value);
		}
	}

}
