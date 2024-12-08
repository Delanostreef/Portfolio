using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{


	[SerializeField] private GameObject explosion;
	[SerializeField] private Vector3 explosionOffset;
	[SerializeField] private float angle;
	private Animator animator;




	private void Start()
	{
		animator = GetComponent<Animator>();
	}






	private void OnTriggerEnter2D( Collider2D collision )
	{
		if (!collision.CompareTag("Player")) return;

		animator.SetBool("pressed", true);
	}



	private void OnTriggerExit2D( Collider2D collision )
	{
		if (!collision.CompareTag("Player")) return;

		animator.SetBool("pressed", false);
		GameObject obj = Instantiate(explosion);
		obj.transform.position = transform.position + explosionOffset;
		obj.transform.Rotate(new Vector3(0,0,angle));
	}



	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position + explosionOffset, .5f);
	}


}
