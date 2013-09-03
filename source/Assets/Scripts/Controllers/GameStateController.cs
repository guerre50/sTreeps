using UnityEngine;
using System.Collections;

public class GameStateController : Singleton<GameStateController> {
	GameStateBehaviour _gameState;
	GameStateBehaviour _oldGameState;
	
	void Start () {
		_gameState = CreateState(GameState.Intro);
		SetState(GameState.Playing);
	}
	
	void SetState(GameState state) {
		RemoveState(_gameState);
		_gameState = CreateState(state);
	}
	
	GameStateBehaviour CreateState(GameState state) {
		GameStateBehaviour behaviour = GameStateFactory.Create(GameState.Intro);
		behaviour.transform.parent = transform;
		
		return behaviour;
	}
	
	void RemoveState(GameStateBehaviour state) {
		_oldGameState = state;
		_.When(state.Outro()).Done(DeleteState);
	}

	void DeleteState() {
		Destroy(_oldGameState.gameObject);
	}
		
	
	GameState GetGameState() {
		return _gameState.gameState;
	}
}
