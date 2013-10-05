using UnityEngine;
using System.Collections;

public enum PersonageType {
	Young,
	Cactus,
	Bonsai,
	Animal
};

public static class PersonageFactory {
	public static Personage Create(PersonageType personageType) {
		System.Type personage;
		
		switch (personageType) {
			case PersonageType.Young:
				personage = typeof(Young);
				break;
			case PersonageType.Cactus:
				personage = typeof(Cactus);
				break;
			case PersonageType.Bonsai:
				personage = typeof(Bonsai);
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
	public Personage Right;
	public Personage Left;
	public PersonageType type;
	public FaceAnimator face;
	protected Clips clips; 
	
	public int Layer {
		get {
			return gameObject.layer;
		}
	}
	
	public virtual AudioClip[] ReleaseClips() {	
		return new AudioClip[0];
	}
	
	public void Start() {
		clips = Clips.instance;
		face = GameObject.Find(type+"Face").GetComponent<FaceAnimator>();
	}
}
