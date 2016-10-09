using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioMgr : MonoBehaviour {
	public AudioClip[] sounds;
	List<AudioSource> audioSources = new List<AudioSource>();

	public AudioSource bgmSource;
	public AudioClip [] bgm;
	AudioSource _audio;
	public bool isMute;

	//DEBUG
	public List<AudioClip []> sampleSounds = new List<AudioClip []>();

	public AudioClip [] SE_GOOD;
	public AudioClip [] SE_BAD;
	public AudioClip [] SE_UP;
	public AudioClip [] SE_NO;
	public AudioClip [] SE_BUTTON;

	public List<int> sampleSoundsDirector;

	// Use this for initialization
	void Start () {
		_audio = GetComponent<AudioSource>();

		for (int i = 0; i < sounds.Length; i++) {
			AudioSource aSource = this.gameObject.AddComponent<AudioSource>();
			audioSources.Add (aSource);
		}

		sampleSounds.Add (SE_GOOD);
		sampleSounds.Add (SE_BAD);
		sampleSounds.Add (SE_UP);
		sampleSounds.Add (SE_NO);
		sampleSounds.Add (SE_BUTTON);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(int id) {
		if (isMute) {
			return;
		}
		audioSources[id].clip = sounds[id];
		audioSources[id].Play();
	}

	public void _play (int id) { // id = sampleSounds[i]
		if (isMute) {
			return;
		}
		_audio.clip = sampleSounds [id] [sampleSoundsDirector [id]];
		_audio.Play ();		
	}

	public void playBGM () {
		bgmSource.clip = bgm[0];
		bgmSource.volume = Const.BGM_VOLUME;
		bgmSource.loop = true;
		bgmSource.Play ();
	}

	public void muteBgm (bool pFlg) {
		bgmSource.mute = pFlg;
	}
}
