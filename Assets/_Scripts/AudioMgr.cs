using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour {
	public AudioClip[] sounds;
	AudioSource _audio;

	// Use this for initialization
	void Start () {
		_audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(int id) {
		_audio.clip = sounds[id];
		_audio.Play();
	}
}
