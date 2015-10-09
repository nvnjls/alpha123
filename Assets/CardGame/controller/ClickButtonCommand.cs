using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class ClickButtonCommand : Command {
		
		[Inject]
		public IHUDViewManager manager {get; set;}

		[Inject]
		public string btnName{ get; set; }

		public override void Execute() {
			manager.OnButtonSelected(btnName);
		}
		
	}
}