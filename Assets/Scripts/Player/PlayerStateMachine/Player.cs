using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    //I might want to create different scriptable player to have different animations for each, If I have some spare team I-ll do it
    public Animator anim;

    [SerializeField] private LayerMask whatIsCard;
    private int cardLayerNumber;

    public List<CardObject> cardObjectList; //list of the cards that the player has while playing, maybe I can take a reference only to the value
    public int numberOfCards; //keep the numbher of the card drawn by the player
    private int cardSum;

    public int stopAskingAtPoints = 20; //above 20 points the player won't ask for more cards
    public int askingPercentage = 7; //percentage out of ten, so with 7 is 70%
    public int maxBlackJack = 21;

    public bool askingForCard;
    public bool decidedWhatToDo;
    public bool receivedCard;
    public bool waitingForNextTurn; //have to know if the player has no action to do to handle the start of the player turn in playerManager

    private string currentAnim;

    public Transform cardPosition; //used to place the cards on the table
    public float cardXOffset; //offset for the x position for each card that is given to the player
    private float currentXOffset; //current offset to apply to the card position

    #region State variables

    public PlayerStateMachine stateMachine{get; private set;}

    public PlayerIdleState idleState{get; private set;}
    public PlayerLostState loseState{get; private set;}
    public PlayerWinState winState{get; private set;}
    public PlayerDecisionState decisionState{get; private set;}
    public PlayerWaitingForCardState waitingForCardState{get; private set;}
    public PlayerRejectCardState rejectCardState{get; private set;}

    #endregion

    #region UI

    public Text cardSumText;

    #endregion

    //Initialize All the different states of the player
    private void Awake(){
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "idle");
        loseState = new PlayerLostState(this, stateMachine, "lose");
        winState = new PlayerWinState(this, stateMachine, "win"); 
        waitingForCardState = new PlayerWaitingForCardState(this, stateMachine, "asking"); 
        rejectCardState = new PlayerRejectCardState(this, stateMachine, "notAsking");
        decisionState = new PlayerDecisionState(this, stateMachine, "idle");
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        askingForCard = false;
        decidedWhatToDo = false;
        receivedCard = false;

        cardObjectList = new List<CardObject>();
        cardLayerNumber = Mathf.RoundToInt(Mathf.Log(whatIsCard.value, 2));
        cardSum = 0;

        currentXOffset = 0;

        //At the start of the game all the player want a card
        stateMachine.Initialize(waitingForCardState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    //reset all the boolean to false since it's the dealer turn
    public void ResetPlayerDecision(){
        decidedWhatToDo = false;
        askingForCard = false;
        receivedCard = false;
        waitingForNextTurn = false;

        //if the player has decided not to take cards then he cannot change his mind at the next draw
        if(stateMachine.currentState != rejectCardState && stateMachine.currentState != loseState){
            stateMachine.ChangeState(decisionState);
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
        if(cardSum >= stopAskingAtPoints){
            return true;
        }else{
            return false;
        }
    }

    public int GetCardSum(){
        return cardSum;
    }

    //used to check if a card hits the player, if it does and the player was waiting for one then add it to the player
    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer == cardLayerNumber){
            Debug.Log("Card hit");
            if(CheckAssignCard()){
                AddCardToPlayerHand(collision.gameObject);
            }
        }
    }

    //Add the card to the player and remove the forces to that card in order to position it on the table
    public void AddCardToPlayerHand(GameObject CardGameObject){
        CardObject cardObject = CardGameObject.GetComponent<CardObject>();

        //Add card object to the list of the cards of the player 
        cardObjectList.Add(cardObject);

        UpdateCardSum(cardObject.GetValue());

        //remove the velocity from the rigidbody
        cardObject.SetVelocity(Vector3.zero);

        //should move the card in the direction of its local axis
        //Send card on the table in front of player faced up, (could move it slowly there with a transform.translate and removing rb forces)
        CardGameObject.transform.position = new Vector3 (cardPosition.position.x + currentXOffset, cardPosition.position.y, cardPosition.position.z);
        CardGameObject.transform.rotation = cardPosition.rotation;

        //deActivate the card so that the player can't pick it up again
        cardObject.RemoveInteraction();

        receivedCard = true;
        askingForCard = false;
        currentXOffset = currentXOffset + cardXOffset;
        FinishTurn();
    }

    public void UpdateCardSum(int value){
        cardSum = cardSum + value;

        cardSumText.text = cardSum.ToString();
    }

    //function called by the card if dragged onto the player
    public void AssignCard(GameObject cardGO){
        if(CheckAssignCard()){
            AddCardToPlayerHand(cardGO);
            //cardObject.RemoveInteraction();
        }
    }

    #region Checks

    //check if the player already received a card in this turn
    public bool CheckReceivedCard(){
        if(receivedCard){
            return true;
        }else{
            return false;
        }
    }

    //function used to check whether or not you can assign card to the player
    public bool CheckAssignCard(){
        if(!receivedCard && askingForCard){
            return true;
        }else{
            return false;
        }
    }

    #endregion

}
