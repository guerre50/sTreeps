using UnityEngine;
using System.Collections;


public enum GameState {
	Intro,
	Playing,
	Outro
}

public static class GameStateFactory {
	public static GameStateBehaviour Create(GameState gameState) {
		System.Type behaviour;
		
		switch (gameState) {
			case GameState.Intro:
				behaviour = typeof(Intro);
				break;
			case GameState.Playing:
				behaviour = typeof(Playing);
				break;
			case GameState.Outro:
				behaviour = typeof(Outro);
				break;
			default:
				behaviour = typeof(Idle);
				break;
		}
		
		return new GameObject(behaviour.Name).AddComponent(behaviour) as GameStateBehaviour;
	}
};

public abstract class GameStateBehaviour : MonoBehaviour {
	public GameState gameState;
	private Deferred _outroFinished;
	private float time;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - time > 2) {
			OutroFinished();
			time = Mathf.Infinity;
		}
	}
	
	public virtual Deferred Outro() {
		_outroFinished = new Deferred();
		time = Time.time;
		
		return _outroFinished;
	}

	private void OutroFinished() {
		if (_outroFinished != null) {
			_outroFinished.Resolve();
			_outroFinished = null;
		}
	}
};

public class Idle: GameStateBehaviour {
};
