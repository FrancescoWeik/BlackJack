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
    private int cardSum;

    public int stopAskingAtPoints = 20; //above 20 points the player won't ask for more cards
    public int askingPercentage = 7; //percentage out of ten, so with 7 is 70%

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

        //Debug.Log(stateMachine.currentState);
    }

    //reset all the boolean to false since it's the dealer turn
    public void ResetPlayerDecision(){
        decidedWhatToDo = false;
        askingForCard = false;
        receivedCard = false;
        waitingForNextTurn = false;

        //if the player has decided not to take cards then he cannot change his mind at the next draw
        if(stateMachine.currentState != rejectCardState){
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
            if(!receivedCard && askingForCard){
                
                CardObject cardObject = collision.gameObject.GetComponent<CardObject>();

                //Add card object to the list of the cards of the player 
                cardObjectList.Add(cardObject);

                UpdateCardSum(cardObject.GetValue());

                //Send card on the table in front of player faced up, (could move it slowly there with a transform.translate and removing rb forces)
                cardObject.SetVelocity(Vector3.zero); //remove the velocity from the rigidbody
                collision.gameObject.transform.position = cardPosition.position;
                collision.gameObject.transform.rotation = cardPosition.rotation;

                // ChangeAnim("idle");
                receivedCard = true;
                askingForCard = false;
                FinishTurn();
            }
        }
    }

    public void UpdateCardSum(int value){
        cardSum = cardSum + value;

        cardSumText.text = cardSum.ToString();
    }

    #region Checks

    public bool CheckReceivedCard(){
        if(receivedCard){
            return true;
        }else{
            return false;
        }
    }

    #endregion

}
