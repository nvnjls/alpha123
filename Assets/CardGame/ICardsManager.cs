namespace strange.examples.CardGame {
	public interface ICardsManager {

		void CardSelected(UiCard selectedCard);
		UiCard GetSelectedCard();
		void SetGameMode(CardManager.Type mode);
		void StartGame();
		void OnCheck();
	}
}