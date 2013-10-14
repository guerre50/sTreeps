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
	protected Deferred _outroFinished;
	private Callback _next;
	private GameStateController _gameStateController;

	void Awake () {
		_gameStateController = GameStateController.instance;
	}
	
	void LateUpdate () {
	}
	
	public Deferred Outro() {
		_outroFinished = new Deferred();
		OnOutro();
		return _outroFinished;
	}
	
	public virtual void OnOutro() {
		
	}
	
	public void Next(Callback Next) {
		_next = Next;
	}
	
	protected void Next() {
		if (_next != null) {
			_next();	
		}
	}

	protected void OutroFinished() {
		if (_outroFinished != null) {
			_outroFinished.Resolve();
			_outroFinished = null;
		}
	}
};

public class Idle: GameStateBehaviour {
};
