using System;

using UnityEngine;

using strange.extensions.context.impl;

namespace strange.examples.CardGame {
	public class CardGameContext : SignalContext {
		
		/**
         * Constructor
         */
		public CardGameContext(MonoBehaviour contextView) : base(contextView) {
		}
		
		protected override void mapBindings() {
			base.mapBindings();
			
			// we bind a command to StartSignal since it is invoked by SignalContext (the parent class) on Launch()
			commandBinder.Bind<StartSignal>().To<CardGameStartCommand>().Pooled();
			commandBinder.Bind<CardSelectedSignal>().To<SelectCardCommand>().Pooled();
			commandBinder.Bind<CheckCardsSignal>().To<CheckCardsCommand>().Pooled();
			commandBinder.Bind<SetGameModeSignal>().To<SetGameModeCommand>().Pooled();
			commandBinder.Bind<ShowResultSignal>().To<ShowResultCommand>().Pooled();
			commandBinder.Bind<ShowWarningSignal>().To<ShowWarningCommand>().Pooled();
			commandBinder.Bind<UpdatePlayerScoreSignal>().To<UpdatePlayerScoreCommand>().Pooled();
			commandBinder.Bind<UpdateAIScoreSignal>().To<UpdateAIScoreCommand>().Pooled();

            CardManager cardManager = GameObject.Find("CanvasObj").GetComponent<CardManager>();
            ResultManager resultManager = GameObject.Find("CanvasObj").GetComponent<ResultManager>();
            injectionBinder.Bind<ICardsManager>().ToValue(cardManager);
            injectionBinder.Bind<IResultManager>().ToValue(resultManager);
			HUDManager viewmanager = cardManager.GetComponentInChildren<HUDManager>();
			injectionBinder.Bind<IHUDViewManager>().ToValue(viewmanager);
		}
		
	}
}