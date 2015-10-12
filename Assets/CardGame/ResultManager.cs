using UnityEngine;
using System.Collections;
namespace strange.examples.CardGame
{
    public class ResultManager : MonoBehaviour,IResultManager
    {
        [Inject]
        public ShowResultSignal showResult { get; set; }

        [Inject]
        public ShowWarningSignal showWarning { get; set; }

        public GameObject PlayerCards;

        [Inject]
        public ICardsManager cardManager { get; set; }

        public UiCard[] GetPlayerCards()
        {
            return PlayerCards.GetComponentsInChildren<UiCard>();
        }

        public void OnCheck()
        {
            if ((cardManager.pSelectedCard == null && GetPlayerCards().Length > 1) || (cardManager.pSelectedCard != null && cardManager.pSelectedCard.pHasChecked))
            {
                Debug.Log("Please select a card");
                //_WarningText.gameObject.SetActive(true);
                showWarning.Dispatch(true);
                return;
            }
            cardManager.mCheckCount++;
            if (cardManager.pSelectedCard == null)
                cardManager.pSelectedCard = cardManager.GetMaxPriorityCard(cardManager.GetPlayerCards());
            cardManager.pSelectedCard.pHasChecked = true;
            if (cardManager.GetLeastHigherPriorityAICard(cardManager.pSelectedCard) != null)
            {
                cardManager.mAIScore++;
                Debug.Log("Computer Won");
            }
            else
            {
                cardManager.mPlayerScore++;
                Debug.Log("Pllayer Won");
            }
            cardManager.UpdateScores();
            if (cardManager.mCheckCount >= 3 || cardManager.pCurrentGameMode._IsSingleStep)
                OnSubmitScores();
            cardManager.pSelectedCard.ResetPosition();
            cardManager.pSelectedCard = null;
        }

        public void OnSubmitScores()
        {
            cardManager.ShowAICards();
            if (cardManager.mAIScore > cardManager.mPlayerScore)
            {
                showResult.Dispatch(true, "You Lost");

                Debug.Log("Computer score is higher");
            }
            else if (cardManager.mAIScore < cardManager.mPlayerScore)
            {
                showResult.Dispatch(true, "You Won");

                Debug.Log("Player score is higher");
            }
            else
            {
                showResult.Dispatch(true, "Match Tie");

                Debug.Log("Both are equal");
            }
        }


    }



}