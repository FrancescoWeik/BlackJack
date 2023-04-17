using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerHand : MonoBehaviour
{
    public List<CardObject> cardObjectList; //list of the cards that the dealer has
    private int cardSum; //sum of the cards
    private int shownCardSum; //sum of the cards of the dealer with the exceeption of the second one (the second card dealt is not shown in blackjack)
    private int numberOfAces; //number of aces held by the dealer
    private int numberOfCards; //number of cards held by the dealer

    public int maxPointsNumber; //number after which dealer cannot draw

    public Transform cardPosition;
    public float cardXOffset;
    private float currentXOffset;

    public Text cardSumText;

    //public bool ShowAllCards; //set by the programmer

    private void Start(){
        numberOfCards = 0;
        numberOfAces = 0;
        cardSum = 0;
    }

    //Give card to the dealer
    public void AssignCard(GameObject cardGO){
        if(cardSum<maxPointsNumber){
            CardObject cardObject = cardGO.GetComponent<CardObject>();
            cardObject.SetAssigned();

            cardObjectList.Add(cardObject);

            cardObject.SetVelocity(Vector3.zero);

            //deActivate the card so that the player can't pick it up again
            cardObject.RemoveInteraction();

            UpdateCardSum(cardObject.GetValue());

            //Send card on the table in front of dealer
            cardGO.transform.position = new Vector3 (cardPosition.position.x + currentXOffset, cardPosition.position.y, cardPosition.position.z);
            
            //Check if it's the second card, if it is then place it face down. Else place it faced up
            if(numberOfCards == 2 && cardSum < 21){
                Quaternion cardRotation = cardPosition.rotation;
                cardRotation.eulerAngles = new Vector3(cardRotation.eulerAngles.x, cardRotation.eulerAngles.y, 180);
                cardGO.transform.rotation = cardRotation;
            }else{
                cardGO.transform.rotation = cardPosition.rotation;
            }

            currentXOffset = currentXOffset + cardXOffset;
        }
    }

    //check if the dealer can draw cards
    public bool CheckAssignCard(){
        //check if it's the dealer turn (in the rules the dealer is the last one to draw), if it's not then return false
        if(GameManager.Instance.isPlayerTurn){
            return false;
        }

        //check if points are more or equals to 17, if they are then you can't draw
        if(cardSum>=maxPointsNumber){
            return false;
        }

        return true;
    }

    public bool CheckCanDraw(){
        if(cardSum>=maxPointsNumber){
            //if dealer only have 2 cards and it's the third turn then flip the second card
            if(numberOfCards == 2){
                FlipCard();
            }
            return false;
        }else{
            return true;
        }
    }

    //Updates the dealer card sum and number of cards.
    public void UpdateCardSum(int value){
        cardSum = cardSum + value;
        numberOfCards++;

        UpdateShownCardSum(value);

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
            GameManager.Instance.StartPlayerTurn();
        }

        cardSumText.text = cardSum.ToString();
    }

    //function used to update the value shown to the user
    private void UpdateShownCardSum(int value){
        //the second card that the dealer deals to himself is not shown
        if(numberOfCards == 2){
            //Do nothing
        }else if(numberOfCards==3){
            //at the end on the third round all the cards are shown
            shownCardSum = cardSum;

            //Display second card
            FlipCard();

        }else{
            //else add the value to the shown card (first turn)
            shownCardSum = shownCardSum + value;
        }
    }

    //rotate the second card faced up
    public void FlipCard(){ 
        Quaternion cardRotation = cardObjectList[1].gameObject.transform.rotation;
        cardRotation.eulerAngles = new Vector3(cardRotation.eulerAngles.x, cardRotation.eulerAngles.y, 0);
        cardObjectList[1].gameObject.transform.rotation = cardRotation;
        shownCardSum = cardSum; //player can see all the cards of the dealer now, so update the shownCardSum

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

    //Check if dealer has any aces
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

    public int GetDealerShownCardSum(){
        return shownCardSum;
    }

    public void ResetToStart(){
        cardObjectList = new List<CardObject>();
        currentXOffset = 0;
        cardSum = 0;
        shownCardSum = 0;
        numberOfAces = 0;
        numberOfCards = 0;
        cardSumText.text = cardSum.ToString();
    }
}
