using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class YawnPlayer : Reactable {
	public AudioClip sound;
	
	void Start () {
		Logic.instance.daytimeHandler += DayTimeListener;
	}
	
	void DayTimeListener(DayTime previous, DayTime daytime) {
		if (!selected) return;
		
		if (previous == DayTime.Night && daytime == DayTime.Day)  {
			audio.clip = sound;
			audio.Play();
		}
	}
	
}
