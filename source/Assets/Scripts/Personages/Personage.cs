using UnityEngine;
using System.Collections;

public enum PersonageType {
	Boy,
	Girl,
	Robot,
	Animal
};

public static class PersonageFactory {
	public static Personage Create(PersonageType personageType) {
		System.Type personage;
		
		switch (personageType) {
			case PersonageType.Boy:
				personage = typeof(Boy);
				break;
			case PersonageType.Girl:
				personage = typeof(Girl);
				break;
			case PersonageType.Robot:
				personage = typeof(Robot);
				break;
			case PersonageType.Animal:
				personage = typeof(Animal);
				break;
			default:
				personage = typeof(Animal);
				break;
		}
		GameObject personageGO = GameObject.Instantiate(Resources.Load("Prefabs/Personages/" + personage.Name), Vector3.zero, Quaternion.identity) as GameObject;
		
		return personageGO.GetComponent(personage.Name) as Personage;
	}
};

public abstract class Personage : MonoBehaviour {
	PersonageCamera _camera;
	public Personage Right;
	public Personage Left;
	
	void Awake() {
		_camera = gameObject.GetComponent<PersonageCamera>() as PersonageCamera;
	}
	
	public int Layer {
		get {
			return 1 << gameObject.layer;
		}
	}
	
	void Update () {
	
	}
}
