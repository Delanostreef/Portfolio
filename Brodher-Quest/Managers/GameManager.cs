using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{

	private PlayerController player;
	private Checkpoint[] checkpoints;


	//
	//	Unity Events
	//


	private void Start()
	{
		checkpoints = FindObjectsOfType<Checkpoint>();
		player = FindObjectOfType<PlayerController>();

		MusicManager.instance.PlayGameSong();
	}

	
	//
	//	Methods
	//


	public void RestartLevel()
	{
		foreach (Checkpoint cp in checkpoints)
			cp.state = Checkpoint.States.Unused;

		player.Restart();
	}

    public void PauseGame()
    {
		Time.timeScale = 0;
    }

	public void ResumeGame()
    {
		Time.timeScale = 1;
    }
}
