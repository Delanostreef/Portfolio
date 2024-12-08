using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : MonoBehaviour
{
	[SerializeField] private Vector2 rayOffset;
	[SerializeField] private LayerMask mask;



	private void Start()
	{
		transform.position = GetTargetPosition();
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(GetTargetPosition(), new Vector2(.7f, .8f));
	}


	public Vector2 GetTargetPosition()
		=> rayOffset + Physics2D.Raycast(transform.position, Vector2.down, 20, mask).point;


}
