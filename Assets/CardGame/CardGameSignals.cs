using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

namespace strange.examples.CardGame
{
	public class StartSignal : Signal {}
	public class CardSelectedSignal : Signal<UiCard> {}
	public class CheckCardsSignal : Signal {}
	public class SetGameModeSignal : Signal<CardManager.Type> {}
	public class ShowWarningSignal : Signal<bool> {}
	public class ShowResultSignal : Signal<bool , string> {}
	public class UpdatePlayerScoreSignal : Signal<int> {}
	public class UpdateAIScoreSignal : Signal<int> {}
}
