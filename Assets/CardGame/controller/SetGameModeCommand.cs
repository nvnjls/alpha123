using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class SetGameModeCommand : Command {

		[Inject]
		public ICardsManager manager { get; set; }

		[Inject]
		public MyType type { get; set; }

		public override void Execute() {
			// perform all game start setup here
			manager.SetGameMode (type);
		}
		
	}
}