using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

	public enum Direction
	{
		Home = -1,
		Target = 1
	}


	[Header("Properties")]
	[SerializeField] private float m_duration;
	[SerializeField] private Direction m_direction;
	[SerializeField] private Transform platform;

	[Header("Positions")]
	[SerializeField] private Vector2 m_startPosition;
	[SerializeField] private Vector2 m_endPosition;


	[Header("Data")]
	[SerializeField] private float timer;



	private void Update()
	{

		timer += Time.deltaTime * (int)m_direction / m_duration;

		//	Checking and syncing

		if (timer > 1) {
			m_direction = Direction.Home;
			timer -= (timer -1) * 2f;
		}

		else if (timer < 0) {
			m_direction = Direction.Target;
			timer += -timer * 2f;
		}

		platform.position = Vector2.Lerp(position + m_startPosition, position + m_endPosition, timer);
	}



	private void OnDrawGizmos()
	{

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(position + m_startPosition, Vector2.one*.5f);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(Vector2.Lerp(position + m_startPosition, position + m_endPosition, timer), .15f);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(position + m_startPosition, position + m_endPosition);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(position + m_endPosition, Vector2.one * .5f);

	}



	//
	//	Getters and Setters
	//

	public Vector2 position
	{
		get => transform.position;
	}

	public Direction direction { get => m_direction; }

}
