using UnityEngine;
using System.Collections;

public class GameStateController : Singleton<GameStateController> {
	GameStateBehaviour _gameState;
	GameStateBehaviour _oldGameState;
	public GameState[] gameStateOrder;
	private int _state = 0;
	
	void Start () {
		_gameState = CreateState(gameStateOrder[_state]);
		_gameState.Next(OnGameStateFinish);
	}
	
	public void SetState(GameState state) {
		RemoveState(_gameState);
		_gameState = CreateState(state);
	}
	
	GameStateBehaviour CreateState(GameState state) {
		GameStateBehaviour behaviour = GameStateFactory.Create(state);
		behaviour.transform.parent = transform;
		
		return behaviour;
	}
	
	void RemoveState(GameStateBehaviour state) {
		_oldGameState = state;
		_.When(state.Outro()).Done(DeleteState);
	}

	void DeleteState() {
		Destroy(_oldGameState.gameObject, 0.0f);
	}
	
	void OnGameStateFinish() {
		_state++;
		SetState(gameStateOrder[_state]);
	}
	
	GameState GetGameState() {
		return _gameState.gameState;
	}
}
