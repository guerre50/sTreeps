using UnityEngine;
using System.Collections;

public class PersonageController : Singleton<PersonageController> {
	CameraController _camera;
	Personage[] _personages;

	void Start() { 
		_camera = CameraController.instance;

		InitPersonages();
	}

	void Update () {
	
	}

	void InitPersonages() {
		_personages = new Personage[2];
		_personages[0] = CreatePersonage(PersonageType.Boy, 0);
		_personages[1] = CreatePersonage(PersonageType.Girl, 1);
	}

	Personage CreatePersonage(PersonageType type, int position) {
		Personage personage = PersonageFactory.Create(type);
		personage.transform.parent = transform;

		return personage;
	}
}
