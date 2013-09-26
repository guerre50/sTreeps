using UnityEngine;
using System.Collections;

//http://stackoverflow.com/questions/949246/how-to-get-all-classes-within-namespace


public class Logic : Singleton<Logic> {
	//private GameStateController _gameState;
	private PersonageController _characterController;
	private StripController _stripController;
	private InputController _inputController;
	
	// TO-DO Move this 
	private enum Weather { Sunny, Cloudy, Rainy};
	private enum DayTime { Day, Night};
	private enum Action { Idle, Spit, Dance, Salute};
		
	private Weather _weather;
	private DayTime _daytime;
	private Action _action = Action.Idle;
	
	void Awake () {
		//_gameState = GameStateController.instance;
		_characterController = PersonageController.instance;
		_stripController = StripController.instance;
		_inputController = InputController.instance;
	}

	void Start() {
		InitGameState();
		InitPersonages();
		InitStrip();
	}
	
	void Update() {
		if (_inputController.Shaking) {
			_stripController.Shake();	
		}
	}

	void InitGameState() {

	}

	void InitPersonages() {

	}

	void InitStrip() {
		_stripController.StripNumber = 5;
	}
	
	public void NightTime() {
		_daytime = DayTime.Night;
		_weather = Weather.Sunny;
		_characterController.NightTime();
	}
	
	public void Sunny() {
		_daytime = DayTime.Day;
		_weather = Weather.Sunny;
		_characterController.Sunny();
	}
	
	public void Rainy() {
		_daytime = DayTime.Day;
		_weather = Weather.Rainy;
		
		_characterController.Rainy();
	}
	
	public void Cloudy() {
		_daytime = DayTime.Day;
		_weather = Weather.Cloudy;
		_characterController.Cloudy();
	}
	
	public bool Spit() {
		if (_daytime != DayTime.Night) {
			_action = Action.Spit;
			_characterController.Spit(ActionFinished);
			return true;
		} else {
			Bother(PersonageType.Bonsai);	
		}
		
		return false;
	}
	
	public void ActionFinished() {
		_action = Action.Idle;	
	}
	
	public void Dance() {
		// TO-DO move this logic to each character
		if (_daytime == DayTime.Day ) {
			if (_weather != Weather.Rainy) {
				_action = Action.Dance;
				_characterController.Dance(ActionFinished);
			}
		} else {
			Bother (PersonageType.Cactus);	
		}
	}
	
	public void Salute() {
		if (_daytime == DayTime.Day) {
			if (_weather != Weather.Rainy) {
				_action = Action.Salute;
				_characterController.Salute();
			}
		} else {
			Bother(PersonageType.Young);	
		}
	}
	
	public void Bother(PersonageType personage) {
		_action = Action.Salute;
		_characterController.BotherSleep(personage);	
	}
	
	public bool IsRainy() {
		return _weather == Weather.Rainy;	
	}
	
	public bool IsNightTime() {
		return _daytime == DayTime.Night;	
	}
}
