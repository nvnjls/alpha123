using System;

using UnityEngine;

using strange.extensions.context.impl;

namespace strange.examples.CardGame {
	public class CardGameBootstrap : ContextView {
		
		void Awake() {
			this.context = new CardGameContext(this);
		}
		
	}
}