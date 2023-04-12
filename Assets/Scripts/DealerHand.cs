using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerHand : MonoBehaviour
{
    public List<CardObject> cardObjectList; //list of the cards that the dealer has
    private int cardSum; //sum of the cards

    public int maxPointsNumber;

    public Transform cardPosition;
    public float cardXOffset;
    private float currentXOffset;

    public Text cardSumText;


    public void AssignCard(GameObject cardGO){
        if(cardSum<maxPointsNumber){
            CardObject cardObject = cardGO.GetComponent<CardObject>();

            cardObjectList.Add(cardObject);
            UpdateCardSum(cardObject.GetValue());

            cardObject.SetVelocity(Vector3.zero);

            //Send card on the table in front of dealer faced up
            cardGO.transform.position = new Vector3 (cardPosition.position.x + currentXOffset, cardPosition.position.y, cardPosition.position.z);
            cardGO.transform.rotation = cardPosition.rotation;

            //deActivate the card so that the player can't pick it up again
            cardObject.RemoveInteraction();

            currentXOffset = currentXOffset + cardXOffset;

            //Start the player turn if a card has been drawn
            PlayersManager.Instance.StartPlayerTurn();
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
        cardSumText.text = cardSum.ToString();
    }
}
