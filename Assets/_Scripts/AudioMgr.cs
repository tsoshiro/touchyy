using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour {
	public AudioClip[] sounds;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(int id) {
		audio.clip = sounds[id];
		audio.Play();
	}
}
