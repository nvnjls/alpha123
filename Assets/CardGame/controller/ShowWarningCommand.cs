using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class ShowWarningCommand : Command {
		
		[Inject]
		public IHUDViewManager manager {get; set;}

		[Inject]
		public bool show{ get; set; }

		public override void Execute() {
			manager.ShowWarning(show);
			Debug.Log ("Show Warning");
		}
		
	}
}