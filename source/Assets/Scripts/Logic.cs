using UnityEngine;
using System.Collections;

//http://stackoverflow.com/questions/949246/how-to-get-all-classes-within-namespace


public class Logic : MonoBehaviour {
	private GameStateController _gameState;
	private PersonageController _characterController;
	private StripController _stripController;
	
	void Awake () {
		_gameState = GameStateController.instance;
		_characterController = PersonageController.instance;
		_stripController = StripController.instance;	
	}

	void Start() {
		InitGameState();
		InitPersonages();
		InitStrip();
	}

	void InitGameState() {

	}

	void InitPersonages() {

	}

	void InitStrip() {
		_stripController.StripNumber = 5;
	}
}
