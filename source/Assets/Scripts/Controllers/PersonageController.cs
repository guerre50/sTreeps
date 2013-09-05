using UnityEngine;
using System.Collections;

public class PersonageController : Singleton<PersonageController> {
	CameraController _camera;
	Personage[] _personages;

	void Awake() { 
		_camera = CameraController.instance;

		InitPersonages();
	}

	void InitPersonages() {
		_personages = new Personage[2];
		_personages[0] = CreatePersonage(PersonageType.Boy, 0);
		_personages[1] = CreatePersonage(PersonageType.Girl, 1);
		
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
}
