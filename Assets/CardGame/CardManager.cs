using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System;

namespace strange.examples.CardGame {
	public class CardManager : ICardsManager {

		[Inject]
		public ShowResultSignal showResult { get; set; }

		[Inject]
		public ShowWarningSignal showWarning { get; set; }

		[Inject]
		public UpdatePlayerScoreSignal updatePlayerScore { get; set; }

		[Inject]
		public UpdateAIScoreSignal updateAIScore { get; set; }


        /*[System.Serializable]
		public class GameMode
		{
			public GameModeType _Type;
			public int _CardsCount;
			public bool _IsSingleStep ;
		}*/
       
        public List<GameMode> _GameModes;
		public string SheetName ;
        public GameObject UiCardObj;
		public GameObject AICards;
		public GameObject PlayerCards;
		public float CardSpaceWidth = 40;
		private Sprite[] sprites;
		//public new GameMode pCurrentGameMode;
		/*
		 * This list will be used to make sure that the Random numbers will not be repeated
		 * */
		public List<int> tempList;
		public static CardManager pInstance;
		//public new UiCard pSelectedCard = null;
		//public new int mPlayerScore , mAIScore , mCheckCount;
		public bool initDone = false;
		
		public override void  StartGame () 
		{
            if (!initDone)
            {
                _GameModes = new List<GameMode>();
                _GameModes.Add(new GameMode { _Type = GameModeType.BEGINNER, _IsSingleStep = true, _CardsCount=1 });
                _GameModes.Add(new GameMode { _Type = GameModeType.INTERMEDIATE, _IsSingleStep = true, _CardsCount = 3 });
                _GameModes.Add(new GameMode { _Type = GameModeType.EXPERT, _IsSingleStep = false, _CardsCount = 3 });
                _GameModes.Add(new GameMode { _Type = GameModeType.PROFESSIONAL, _IsSingleStep = false, _CardsCount = 4 });
                pInstance = this;
				sprites = Resources.LoadAll<Sprite>(SheetName); 
				SetGameMode (GameModeType.BEGINNER);
				tempList = new List<int>();
				initDone = true;
                
			}
			Restart ();		
		}
		public override UiCard GetSelectedCard ()
		{
			return pSelectedCard;
		}

		private int GetRandomInt()
		{
			int rand = UnityEngine.Random.Range (0, 10);
			if (tempList.Contains (rand))
				return GetRandomInt ();
			else {
				tempList.Add (rand);
			}
			return rand;
		}

		/*
		 * This is to create a Card Object and assigning a Random priority .
		 * Here I am assuming that all the sprites in the spritesheet are arranged in Ascending Order.
		 * */
		private GameObject CreateObject(Transform parent , bool isAI = false)
		{
			GameObject obj = (GameObject)Instantiate(UiCardObj, parent.position, Quaternion.identity);
			int rand = GetRandomInt ();
			UiCard cardInfo = obj.GetComponent<UiCard>();
			cardInfo.pPriority = rand;
			if(!isAI)
				cardInfo.SetSprite (sprites [rand]);
			cardInfo.IsAI = isAI;
			obj.transform.parent = parent;
			return obj;
		}

		public override void CardSelected(UiCard selectedCard)
		{
			UiCard []playerCards = PlayerCards.GetComponentsInChildren<UiCard> ();
			pSelectedCard = selectedCard;
			foreach (UiCard card in playerCards)
				if (card.pPriority != selectedCard.pPriority)
					card.ResetPosition ();
			showWarning.Dispatch (false);
		}

		
		private void GenerateCards(int count , bool mIsAICard)
		{
			Transform parentTransform;
			if (mIsAICard)
				parentTransform = AICards.transform;
			else
				parentTransform = PlayerCards.transform;
			for(int i =0 ; i < count; i++)
			{
				GameObject obj = CreateObject (parentTransform , mIsAICard);
				obj.transform.position = new Vector3(obj.transform.position.x + i  * CardSpaceWidth , obj.transform.position.y , obj.transform.position.z);
			}
		}

		/*
		 * This is to Show the AI cards to the User after the player clicks of Submit.
		 * */
		public override void ShowAICards()
		{
			UiCard [] cards = GetAICards ();
			foreach(UiCard card in cards)
				card.SetSprite (sprites [card.pPriority]);
		}
		public void OnSubmitScores()
		{
			ShowAICards ();
			if (mAIScore > mPlayerScore)
			{
				showResult.Dispatch (true , "You Lost");

				Debug.Log ("Computer score is higher");
			}
			else if (mAIScore < mPlayerScore)
			{
				showResult.Dispatch (true , "You Won");

				Debug.Log ("Player score is higher");
			}
			else
			{
				showResult.Dispatch (true , "Match Tie");

				Debug.Log ("Both are equal");
			}
		}

		public UiCard[] GetAICards()
		{
			return  AICards.GetComponentsInChildren<UiCard> ();
		}
		public override UiCard[] GetPlayerCards()
		{
			return  PlayerCards.GetComponentsInChildren<UiCard> ();
		}


		public override void OnCheck()
		{
            
			if ((pSelectedCard == null && GetPlayerCards ().Length > 1) || (pSelectedCard != null && pSelectedCard.pHasChecked))
			{
				Debug.Log ("Please select a card");
				//_WarningText.gameObject.SetActive(true);
				showWarning.Dispatch(true);
				return;
			}
			mCheckCount++; 
			if(pSelectedCard == null)
				pSelectedCard = GetMaxPriorityCard (GetPlayerCards());
			pSelectedCard.pHasChecked = true;
			if (GetLeastHigherPriorityAICard(pSelectedCard) != null)
			{
				mAIScore ++;
				Debug.Log ("Computer Won");
			}
			else 
			{
				mPlayerScore ++;
				Debug.Log ("Pllayer Won");
			}
			UpdateScores ();
			if (mCheckCount >= 3 || pCurrentGameMode._IsSingleStep)
				OnSubmitScores ();
			pSelectedCard.ResetPosition();
			pSelectedCard = null;
		}
		public override void UpdateScores()
		{
			updatePlayerScore.Dispatch (mPlayerScore);
			updateAIScore.Dispatch (mAIScore);
		}

		public override UiCard GetLeastHigherPriorityAICard(UiCard selectedCard)
		{
			UiCard [] cards = GetAICards ();
			UiCard LeastHigherPriorityCard = null;
			foreach(UiCard card in cards)
			{
				if(card.pHasChecked)
					continue;
				if(card.pPriority > selectedCard.pPriority)
				{
					if(LeastHigherPriorityCard == null)
						LeastHigherPriorityCard = card;
					else if(LeastHigherPriorityCard.pPriority > card.pPriority)
						LeastHigherPriorityCard = card;
				}
			}
			if(LeastHigherPriorityCard != null)
			{
				LeastHigherPriorityCard.pHasChecked = true;
				LeastHigherPriorityCard.SetSprite(sprites[LeastHigherPriorityCard.pPriority]);
			}
			return LeastHigherPriorityCard;
		}
		/* 
		 * This is to get the Max priority card from the give array of Cards
		 * */
		public override UiCard GetMaxPriorityCard(UiCard []cards)
		{
			UiCard maxCard = cards [0];
			foreach(UiCard card in cards)
			{
				if(maxCard.pPriority < card.pPriority)
					maxCard = card;
			}
			return maxCard;
		}
		private void DeleteAllCards()
		{
			UiCard []aicards = GetAICards();
			UiCard []playerCards = GetPlayerCards();
			foreach (UiCard card in aicards)
				DestroyObject (card.gameObject);
			foreach (UiCard card in playerCards)
				DestroyObject (card.gameObject);

		}
		public override void SetGameMode(GameModeType type)
		{
			foreach (GameMode mode in _GameModes)
				if (mode._Type == type)
					pCurrentGameMode = mode;
		}
	
		public void Restart()
		{
			mPlayerScore = 0;
			mAIScore = 0;
			mCheckCount = 0;
			pSelectedCard = null;
			UpdateScores ();
			//_WonText.gameObject.SetActive (false);
			//_WarningText.gameObject.SetActive(false);
			showWarning.Dispatch(false );
			showResult.Dispatch (false , "");

			tempList.Clear ();
			DeleteAllCards ();
            /* For now we have only 2 players.i.e Player and AI Player(Computer/Mobile) , so hard coding for now
			 * */
            GenerateCards(pCurrentGameMode._CardsCount , true);
            GenerateCards(pCurrentGameMode._CardsCount , false);
           //GenerateCards(true);
           //GenerateCards(false);
		}

       private void GenerateCards(bool mIsAICard)
        {
            int count = pCurrentGameMode._CardsCount;
            Transform parentTransform;
            if (mIsAICard)
                parentTransform = AICards.transform;
            else
                parentTransform = PlayerCards.transform;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = CreateObject(parentTransform, mIsAICard);
                obj.transform.position = new Vector3(obj.transform.position.x + i * CardSpaceWidth, obj.transform.position.y, obj.transform.position.z);
            }

        }
    }
}
