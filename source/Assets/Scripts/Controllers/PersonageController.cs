using UnityEngine;
using System.Collections;

public class PersonageController : Singleton<PersonageController> {
	CameraController _camera;
	Personage[] _personages;
	public GameObject sTreeps;
	private Logic _logic;
	string _animation;

	void Awake() { 
		_camera = CameraController.instance;
		_logic = Logic.instance;

		InitPersonages();
	}

	void InitPersonages() {
		_personages = new Personage[3];
		_personages[0] = CreatePersonage(PersonageType.Young, 0);
		_personages[1] = CreatePersonage(PersonageType.Cactus, 1);
		_personages[2] = CreatePersonage(PersonageType.Bonsai, 2);
		
		
		int _personagesLength = _personages.Length;
		for (int i = 0; i < _personagesLength; ++i) {
			int prev = (i-1)%_personagesLength;
			if (prev < 0) prev += _personagesLength;
			_personages[i].Left = _personages[prev];
			_personages[i].Right = _personages[(i+1)%_personagesLength];
		}
	}
	
	public Personage RandomPersonage() {
		int random = Random.Range(0, _personages.Length);
		return _personages[random];	
	}

	Personage CreatePersonage(PersonageType type, int position) {
		Personage personage = PersonageFactory.Create(type);
		personage.transform.parent = transform;

		return personage;
	}
	
	private void AnimateYawn(string animation, float fade) {
		if (Yawn()) {
			AnimateQueued(animation, fade);
		} else {
			Animate(animation, fade);	
		}
	}
	
	private bool Yawn() {
		if (_animation == "Yawn") {
			Animate("Wake", 0.5f);	
			return true;
		}
		
		return false;
	}
	
	public void NightTime() {
		Animate("Yawn", 1.0f);
	}
	
	public void Sunny() {
		AnimateYawn("Idle", 1.0f);
	}
	
	public void Cloudy() {
		AnimateYawn("Cloud", 1.0f);
	}
	
	public void Rainy() {
		AnimateYawn("Rain", 1.0f);
	}
	
	public void Dance() {
		string currentAnimation = _animation;
		sTreeps.animation.CrossFade("Dance", 0.5f);
		sTreeps.animation.CrossFadeQueued(currentAnimation, 1.0f);
	}
	
	private void Animate(string clip, float crossFade) {
		_animation = clip;
		sTreeps.animation.CrossFade(clip, crossFade);
	}
	
	private void AnimateQueued(string clip, float crossFade) {
		_animation = clip;
		sTreeps.animation.CrossFadeQueued(clip, crossFade);
	}
}
