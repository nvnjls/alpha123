using UnityEngine;
using System.Collections.Generic;
namespace strange.examples.CardGame {
	public abstract class ICardsManager: MonoBehaviour {

        public UiCard pSelectedCard= null;
        
        public GameMode pCurrentGameMode;
        public int mPlayerScore, mAIScore, mCheckCount;
        public abstract UiCard GetMaxPriorityCard(UiCard [] cards);
        public abstract void UpdateScores();
        public abstract UiCard GetLeastHigherPriorityAICard(UiCard card);
        public abstract void ShowAICards();
        public abstract UiCard[] GetPlayerCards(); 
        public abstract void CardSelected(UiCard selectedCard);
		public abstract UiCard GetSelectedCard();
		public abstract void SetGameMode(GameModeType mode);
		public abstract void StartGame();
		public abstract void OnCheck();
	}
}