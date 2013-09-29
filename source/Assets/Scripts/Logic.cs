using UnityEngine;
using System.Collections;

//http://stackoverflow.com/questions/949246/how-to-get-all-classes-within-namespace



public delegate void WeatherEventHandler(Weather previousWeather, Weather newWeather);
public delegate void DayTimeEventHandler(DayTime previousDaytime, DayTime newDaytime);
public delegate void ActionEventHandler(Action previousAction, Action newAction);


public enum Weather { Sunny, Cloudy, Rainy};
public enum DayTime { Day, Night};
public enum Action { Idle, Spit, Dance, Salute, Bother};

public class Logic : Singleton<Logic> {
	//private GameStateController _gameState;
	private PersonageController _characterController;
	private StripController _stripController;
	private InputController _inputController;
	
	// TO-DO Move this 
		
	private Weather _weather;
	private DayTime _daytime;
	private Action _action = Action.Idle;
	
	public Weather Weather {
		get {
			return _weather;	
		}
		set {
			Weather previous = _weather;
			_weather = value;
			if (weatherHandler != null) weatherHandler(previous, _weather);
		}
	}
	
	public DayTime DayTime {
		get {
			return _daytime;	
		}
		set {
			DayTime previous = _daytime;
			_daytime = value;
			if (daytimeHandler != null) daytimeHandler(previous, _daytime);
		}
	}
	
	public Action Action {
		get {
			return _action;	
		}
		set {
			Action previous = _action;
			_action = value;
			if (actionHandler != null) actionHandler(previous, _action);
		}
	}
	
	public event WeatherEventHandler weatherHandler;
	public event DayTimeEventHandler daytimeHandler;
	public event ActionEventHandler actionHandler;
	
	
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
		DayTime = DayTime.Night;
		Weather = Weather.Sunny;
		_characterController.NightTime();
	}
	
	public void Sunny() {
		DayTime = DayTime.Day;
		Weather = Weather.Sunny;
		_characterController.Sunny();
	}
	
	public void Rainy() {
		DayTime = DayTime.Day;
		Weather = Weather.Rainy;
		
		_characterController.Rainy();
	}
	
	public void Cloudy() {
		DayTime = DayTime.Day;
		Weather = Weather.Cloudy;
		_characterController.Cloudy();
	}
	
	public bool Spit() {
		if (DayTime != DayTime.Night) {
			Action = Action.Spit;
			_characterController.Spit(ActionFinished);
			return true;
		} else {
			Bother(PersonageType.Bonsai);	
		}
		
		return false;
	}
	
	public void ActionFinished() {
		Action = Action.Idle;	
	}
	
	public bool Dance() {
		bool result = false;
		// TO-DO move this logic to each character
		if (DayTime == DayTime.Day ) {
			if (Weather != Weather.Rainy) {
				Action = Action.Dance;
				_characterController.Dance(ActionFinished);
				result = true;
			}
		} else {
			result = Bother (PersonageType.Cactus);	
		}
		
		return result;
	}
	
	public void Salute() {
		if (DayTime == DayTime.Day) {
			if (Weather != Weather.Rainy) {
				Action = Action.Salute;
				_characterController.Salute();
			}
		} else {
			Bother(PersonageType.Young);	
		}
	}
	
	public bool Bother(PersonageType personage) {
		Action = Action.Bother;
		_characterController.BotherSleep(personage);
		
		return true;
	}
	
	public bool IsRainy() {
		return Weather == Weather.Rainy;	
	}
	
	public bool IsNightTime() {
		return DayTime == DayTime.Night;	
	}
}
