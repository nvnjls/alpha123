using System;

using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.examples.CardGame {
	public class ShowResultCommand : Command {
		
		[Inject]
		public IHUDViewManager manager {get; set;}

		[Inject]
		public bool show{ get; set; }

		[Inject]
		public string text{ get; set; }

		public override void Execute() {
			manager.ShowResult(show , text);
		}
		
	}
}