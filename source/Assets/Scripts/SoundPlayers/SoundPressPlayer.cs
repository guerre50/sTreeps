using UnityEngine;
using System.Collections;

public class SoundPressPlayer : Reactable {
	public AudioClip[] clips;
	public bool random = true;
	public Vector2 randomTimeRange = new Vector2(5.0f, 10f);
	
	void Start() {
		if (random) {
			_.Wait(Random.Range(5.0f, 10.0f)).Done (RandomPlay);		
		}
	}
	
	public void OnPressDown(InputInfo input) {
		if (!enabled) return;
		if (!audio.isPlaying) {
			Play(false);
		}
	}
	
	public void RandomPlay() {
		_.Wait(Random.Range(randomTimeRange.x, randomTimeRange.y)).Done (RandomPlay);
		Play (true);
	}
	
	public void Play() {
		Play(false);	
	}
	
	public void Play(bool random) {
		Play (random, -1);
	}
	
	public void Stop() {
		audio.Stop();	
	}
	
	public void Play(bool random, int clip) {
		if (((!enabled && selected) || selected) && (!audio.isPlaying || !random)) {
			audio.Stop();
			if (clip < 0) {
				audio.clip = clips[Random.Range(0, clips.Length)];
			} else {
				audio.clip = clips[clip];	
			}
			audio.Play();	
		}
	}
}
