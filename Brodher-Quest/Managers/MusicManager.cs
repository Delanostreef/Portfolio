using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : SingletonBehaviour<MusicManager>
{
	[SerializeField] private AudioClip m_mainMenuSong;
	[SerializeField] private AudioClip m_playSong;


	private AudioSource source;


	private void Start()
	{
		source = GetComponent<AudioSource>();
		PlayMainMenu();
		DontDestroyOnLoad(gameObject);
	}


	public void PlayMainMenu() => Play(m_mainMenuSong);
	public void PlayGameSong() => Play(m_playSong);


	public void Play(AudioClip clip)
	{
		if (!source || source.clip == clip) return;

		source.Stop();
		source.clip = clip;
		source.Play();
	}
}




