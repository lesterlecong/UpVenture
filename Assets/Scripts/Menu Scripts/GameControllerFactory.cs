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
			switch (gameType) {
				case GameType.Endless:
					gameController = GameController.current;
					break;
				case GameType.Adventure:
					gameController = AdventureGameController.current;
					break;
				default:
					gameController = null;
					break;
			}

			return gameController;

		}

		private GameControllerFactory (){

		}
	}
}

