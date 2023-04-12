using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //I might want to create different scriptable player to have different animations for each, If I have some spare team I-ll do it

    private Animator anim;

    [SerializeField] private LayerMask whatIsCard;
    private int cardLayerNumber;

    public List<CardObject> cardObjectList; //list of the cards that the player has while playing, maybe I can take a reference only to the value

    public int stopAskingAtPoints = 20; //above 20 points the player won't ask for more cards
    public int askingPercentage = 7; //percentage out of ten, so with 7 is 70%

    public bool askingForCard;
    public bool decidedWhatToDo;
    public bool receivedCard;
    public bool waitingForNextTurn; //have to know if the player has no action to do to handle the start of the player turn in playerManager

    private string currentAnim;

    public Transform cardPosition; //used to place the cards on the table

    void Start()
    {
        anim = GetComponent<Animator>();

        askingForCard = false;
        decidedWhatToDo = false;
        receivedCard = false;

        currentAnim = "idle";
        ChangeAnim(currentAnim);

        cardObjectList = new List<CardObject>();
        cardLayerNumber = Mathf.RoundToInt(Mathf.Log(whatIsCard.value, 2));
    }

    void Update()
    {
        if(GameManager.Instance.isPlayerTurn){
            int number = Random.Range(0,10);
            if(number > askingPercentage || ExceedMaxAskingPoints()){
                //Do Not Ask For Card
                askingForCard = false;
                ChangeAnim("notAsking");
                FinishTurn();
            }else{
                //Ask For A Card if player hasn't already received one, else wait for end of turn
                if(!receivedCard){
                    askingForCard = true;
                    ChangeAnim("asking");
                }else{
                    ChangeAnim("idle");
                    FinishTurn();
                }
            }
            decidedWhatToDo = true;
        }
    }

    //reset all the boolean to false since it's the dealer turn
    public void ResetPlayerDecision(){
        decidedWhatToDo = false;
        askingForCard = false;
        receivedCard = false;
        waitingForNextTurn = false;
    }

    //function used to change the character animation
    private void ChangeAnim(string newAnim){
        anim.SetBool(currentAnim, false);
        anim.SetBool(newAnim, true);
        currentAnim = newAnim;
    }

    //used to check if a card hits the player, if it does and the player was waiting for one then add it to the player
    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer == cardLayerNumber){
            Debug.Log("Card hit");
            if(!receivedCard && askingForCard){
                //Add card object to the list of the cards of the player 
                cardObjectList.Add(collision.gameObject.GetComponent<CardObject>());

                //Send card on the table in front of player faced up, (could move it slowly there with a transform.translate and removing rb forces)
                collision.gameObject.transform.position = cardPosition.position;
                collision.gameObject.transform.rotation = cardPosition.rotation;

                ChangeAnim("idle");
                receivedCard = true;
                askingForCard = false;
                FinishTurn();
            }
        }
    }

    public void FinishTurn(){
        //add +1 on playerFinishTurn to playersmanager
        if(!waitingForNextTurn){
            waitingForNextTurn = true;
            PlayersManager.Instance.AddWaitingForTurn();
        }
    }

    public bool ExceedMaxAskingPoints(){
        int cardSum = 0;
        for(int i=0; i<cardObjectList.Count; i++){
            cardSum = cardSum + cardObjectList[i].GetValue();
        }

        Debug.Log("Card sum is " + cardSum);
        
        if(cardSum >= stopAskingAtPoints){
            return true;
        }else{
            return false;
        }
    }
}
