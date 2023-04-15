using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerHand : MonoBehaviour
{
    public List<CardObject> cardObjectList; //list of the cards that the dealer has
    private int cardSum; //sum of the cards
    private int numberOfAces; //number of aces held by the dealer
    private int numberOfCards; //number of cards held by the dealer

    public int maxPointsNumber;

    public Transform cardPosition;
    public float cardXOffset;
    private float currentXOffset;

    public Text cardSumText;

    private void Start(){
        numberOfCards = 0;
        numberOfAces = 0;
        cardSum = 0;
    }


    public void AssignCard(GameObject cardGO){
        if(cardSum<maxPointsNumber){
            CardObject cardObject = cardGO.GetComponent<CardObject>();

            cardObjectList.Add(cardObject);

            cardObject.SetVelocity(Vector3.zero);

            //Send card on the table in front of dealer faced up
            cardGO.transform.position = new Vector3 (cardPosition.position.x + currentXOffset, cardPosition.position.y, cardPosition.position.z);
            cardGO.transform.rotation = cardPosition.rotation;

            //deActivate the card so that the player can't pick it up again
            cardObject.RemoveInteraction();

            currentXOffset = currentXOffset + cardXOffset;

            UpdateCardSum(cardObject.GetValue());
        }
    }

    //check if the dealer can draw cards
    public bool CheckAssignCard(){
        //check if it's the dealer turn (in the rules the dealer is the last one to draw), if it's not then return false
        if(GameManager.Instance.isPlayerTurn){
            return false;
        }

        //check if player has drawn a card, if he did then you can draw, otherwise you can't


        //check if points are more or equals to 17, if they are then you can't draw
        if(cardSum>=maxPointsNumber){
            return false;
        }

        return true;
    }

    public bool CheckCanDraw(){
        if(cardSum>=maxPointsNumber){
            return false;
        }else{
            return true;
        }
    }

    public void UpdateCardSum(int value){
        cardSum = cardSum + value;
        numberOfCards++;

        if(value == 11){
            //it's an ace
            numberOfAces ++;
        }

        //Check if exceeding max blackjack number
        if(cardSum>21){
            CheckIfLost();
        }
        else if(cardSum == 21 && numberOfCards == 2){
            //it's a blackjack so end the game
            GameManager.Instance.EndGame();
        }
        else{
            //Start the player turn if a card has been drawn
            //PlayersManager.Instance.StartPlayerTurn();
            GameManager.Instance.StartPlayerTurn();
        }

        cardSumText.text = cardSum.ToString();
    }

    //Check if the dealer lost, before doing so check if he has aces, if he does then check if dealer loses even if the ace value is 1
    public void CheckIfLost(){
        if(cardSum>21){
            if(CheckAces()){
                cardSum = cardSum - 10; //give the ace a value of 1
                numberOfAces --;
                CheckIfLost(); //Check again if lost anyway, I think it's impossible though
            }else{
                GameManager.Instance.EndGame();
                return;
            }
        }else{
            GameManager.Instance.StartPlayerTurn();
        }
    }

    public bool CheckAces(){
        if(numberOfAces > 0){
            return true;
        }else{
            return false;
        }
    }
    
    public int GetDealerCardSum(){
        return cardSum;
    }

    public void ResetToStart(){
        currentXOffset = 0;
        cardSum = 0;
        numberOfAces = 0;
        numberOfCards = 0;
        cardSumText.text = cardSum.ToString();
    }
}
