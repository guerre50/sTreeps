using UnityEngine;
using System.Collections;

//http://stackoverflow.com/questions/949246/how-to-get-all-classes-within-namespace


public class Logic : Singleton<Logic> {
	private GameStateController _gameState;
	private PersonageController _characterController;
	private StripController _stripController;
	// TO-DO Move this 
	private enum Weather { Sunny, Cloudy, Rainy};
	private enum DayTime { Day, Night};
	private Weather _weather;
	private DayTime _daytime;
	
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
	
	public void Dance() {
		// TO-DO move this logic to each character
		if (_daytime == DayTime.Day && _weather != Weather.Rainy) {
			_characterController.Dance();
		}
	}
	
	public bool IsRainy() {
		return _weather == Weather.Rainy;	
	}
}
