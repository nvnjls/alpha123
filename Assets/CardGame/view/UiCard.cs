using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strange.examples.CardGame {
	public class UiCard : View {

		[Inject]
		public CardSelectedSignal cardSelectedSignal {get; set;}


		[Inject]
		public ICardsManager manager {get; set;}

		private int mPriority;
		private bool mHasChecked;
		public Image spriterender ;
		public bool IsAI = false;
		public Vector3 _SelectedCardOffSet ;
		private Vector3 pos;
		public int pPriority { get { return mPriority; } set { mPriority = value ;} }
		public bool pHasChecked { get { return mHasChecked; } set { mHasChecked = value ;} }

		protected override void Start()
		{
			base.Start ();
			pos = transform.position;
		}
		public void OnCardSelected()
		{
			if (IsAI || pHasChecked || manager.GetSelectedCard() == this)
				return;
			transform.position = new Vector3 (transform.position.x+_SelectedCardOffSet.x , transform.position.y+_SelectedCardOffSet.y , transform.position.z+_SelectedCardOffSet.z);
			cardSelectedSignal.Dispatch (this);
		}

		public void ResetPosition()
		{
			transform.position = pos;
		}

		public void SetSprite(Sprite card)
		{
			spriterender.sprite = card;
		}
	}
}
