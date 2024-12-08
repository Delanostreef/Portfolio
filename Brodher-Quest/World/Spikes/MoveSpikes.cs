using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSpikes : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject spike;
    [SerializeField] Vector3 target;


    private float direction = -1;
    private float timer = 0;
    private Vector3 startpos;


    void Start()
    {
        startpos = spike.transform.position;
    }

    void Update()
    {

        if((timer < 1 && direction == 1) || (timer > 0 && direction == -1))
            timer += Time.deltaTime * speed * direction;

        spike.transform.position = Vector3.Lerp(startpos, startpos + target, timer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            direction = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            direction = -1;
        }
    }


	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spike.transform.position + target, Vector2.one);
	}
}
