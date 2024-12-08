using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{


	[SerializeField] private float destroyAfter;


	private void Start()
	{
		Invoke("Die", destroyAfter);
	}


	private void Die() => Destroy(gameObject);
}
