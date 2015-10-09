using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
namespace strange.examples.CardGame {

	public class HUDManager : View , IHUDViewManager {
		public Text _AIScore, _PlayerScore ,_WonText , _WarningText;


		[Inject]
		public StartSignal startgame { get; set; }

		[Inject]
		public CheckCardsSignal checkCards { get; set; }

		[Inject]
		public SetGameModeSignal setGameMode { get; set; }

		/*
		 * this will be invoked from the UnityUI.button click event
		 * */
		public void OnButtonSelected(string btnName)
		{
			switch(btnName)
			{
				case "Restart" :
					startgame.Dispatch();
					break;
				case "Submit":
					checkCards.Dispatch();
					break;
			}
		}

		/*
		 * this will be invoked from the UnityUI.button click event
		 * */
		public void OnModeSelected(Text Mode)
		{
			switch(Mode.text)
			{
				case "Mode-1" :
					setGameMode.Dispatch(CardManager.Type.BEGINNER);
					break;
				case "Mode-2":
					setGameMode.Dispatch(CardManager.Type.INTERMEDIATE);
					break;
				case "Mode-3":
					setGameMode.Dispatch(CardManager.Type.EXPERT);
					break;
			}
			startgame.Dispatch();
		}

		public void ShowWarning(bool show)
		{
			_WarningText.gameObject.SetActive (show);
		}

		public void ShowResult(bool show , string text = "")
		{
			if (!string.IsNullOrEmpty (text))
				_WonText.text = text;
			_WonText.gameObject.SetActive (show);
		}

		public void UpdatePlayerScore(int score)
		{
			_PlayerScore.text = score.ToString ();
		}
		
		public void UpdateAIScore(int score)
		{
			_AIScore.text = score.ToString ();
		}
	}
}