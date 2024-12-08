using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public enum States
    {
        Unused,
        Active,
        Kamikazi
    }


    [SerializeField] private int damage;
    [SerializeField] private Vector3 checkpointOffset;

    //
    //  Class Variables
    //

    private States m_checkpointState;
    private Animator animator;

    //
    //  Unity Events
    //

    private void Start()
    {
        state = States.Unused;
        animator = GetComponent<Animator>();
    }

	private void OnTriggerEnter2D( Collider2D collision )
	{
        if (!collision.CompareTag("Player")) return;

        switch(state)
		{
            case States.Unused:
                collision.GetComponent<PlayerController>().SetCheckpoint(this);
                break;

            case States.Kamikazi:
                collision.GetComponent<PlayerController>().Damage();
                break;
		}
	}

	//
	//  Getters and Setters
	//

	public States state
    {
        get => m_checkpointState;
        set
        {
            if (m_checkpointState == value) return;

            m_checkpointState = value;
            animator.SetInteger("state", (int)m_checkpointState);
        }
    }

	//
	//  Gizmos
	//

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetPos, Vector3.one);
	}


    public Vector3 targetPos { get => transform.position + checkpointOffset; }
}
