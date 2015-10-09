namespace strange.examples.CardGame {
	public interface IHUDViewManager {

		void OnButtonSelected(string btnName);
		void ShowWarning(bool show);
		void ShowResult(bool show , string text);
		void UpdatePlayerScore(int score);
		void UpdateAIScore(int score);
	}
}