using UnityEngine;
using System.Collections;

public class SnorePlayer : SoundSelectPlayer {
	void Start () {
		Logic.instance.daytimeHandler += DayTimeListener;
	}
	
	void DayTimeListener(DayTime previous, DayTime daytime) {
		bool isNight = daytime == DayTime.Night;
		
		enabled = isNight;
	}
}
