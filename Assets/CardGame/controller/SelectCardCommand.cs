using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class SelectCardCommand : Command {
		
		[Inject]
		public ICardsManager manager {get; set;}

		[Inject]
		public UiCard card{ get; set; }

		public override void Execute() {
			manager.CardSelected(card);
		}
		
	}
}