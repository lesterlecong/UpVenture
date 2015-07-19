using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class GameControllerFactory {
		private static GameControllerFactory _instance = null;

		public static GameControllerFactory Instance(){
			if (_instance == null) {
				_instance = new GameControllerFactory();
			}

			return _instance;
		}

		public GameController controller(GameType gameType){
			GameController gameController;
	
			if (gameType == GameType.Endless) {

				gameController = EndlessGameController.current;

			} else if (gameType == GameType.MountainAdventure || 
				gameType == GameType.CityAdventure ||
				gameType == GameType.BeachAdventure) {

				gameController = AdventureGameController.current;

			}else{

				gameController = null;

			}

			return gameController;

		}

		private GameControllerFactory (){

		}
	}
}

