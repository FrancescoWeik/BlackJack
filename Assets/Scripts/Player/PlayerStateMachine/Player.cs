using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    //I might want to create different scriptable player to have different animations for each, If I have some spare team I-ll do it
    public Animator anim;
    private AudioSource audioSource;

    [SerializeField] private PlayerData playerData; //data holding some player variables.

    public string playerName; 

    [SerializeField] private LayerMask whatIsCard;
    private int cardLayerNumber;

    public List<CardObject> cardObjectList; //list of the cards that the player has while playing, maybe I can take a reference only to the value
    public int numberOfCards; //keep the numbher of the card drawn by the player
    private int cardSum;
    private int numberOfAces; //keep track to wheter or not player has aces

    public int stopAskingAtPoints = 20; //above 20 points the player won't ask for more cards
    public int askingPercentage = 7; //percentage out of ten, so with 7 is 70%
    public int maxBlackJack = 21;

    public DealerHand dealerHand; //variable needed to check the hand of the dealer and compare it to yourself

    public bool askingForCard;
    public bool decidedWhatToDo;
    public bool receivedCard;
    public bool waitingForNextTurn; //have to know if the player has no action to do to handle the start of the player turn in playerManager
    public bool lost; //keep track if a player lost the game
    public bool rejectCards; //keep track if a player rejects cards

    private string currentAnim;

    public Transform cardPosition; //used to place the cards on the table
    public float cardXOffset; //offset for the x position for each card that is given to the player
    private float currentXOffset; //current offset to apply to the card position

    public bool showStats;

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
    [SerializeField] private Text playerNameText;
    [SerializeField] private GameObject statsCanvas;
    [SerializeField] private Text statsPlayerName;
    [SerializeField] private Text statsDrawPercentage;
    [SerializeField] private Text statsCurrentState;

    #endregion

    //Initialize All the different states of the player
    private void Awake(){
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        loseState = new PlayerLostState(this, stateMachine, playerData, "lose");
        winState = new PlayerWinState(this, stateMachine, playerData, "win"); 
        waitingForCardState = new PlayerWaitingForCardState(this, stateMachine, playerData, "asking"); 
        rejectCardState = new PlayerRejectCardState(this, stateMachine, playerData, "notAsking");
        decisionState = new PlayerDecisionState(this, stateMachine, playerData, "idle");
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        gameObject.name = playerName;
        playerNameText.text = playerName;

        askingForCard = false;
        decidedWhatToDo = false;
        receivedCard = false;

        cardObjectList = new List<CardObject>();
        cardLayerNumber = Mathf.RoundToInt(Mathf.Log(whatIsCard.value, 2));
        cardSum = 0;
        numberOfAces = 0;

        currentXOffset = 0;

        //get the dealer hand at the start of the game
        dealerHand = GameObject.Find("DealerField").GetComponent<DealerHand>();

        showStats = false;

        //At the start of the game all the player want a card
        stateMachine.Initialize(waitingForCardState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    //reset all the boolean to false since it's the start of the player turn, unless player has already lost or can't ask for cards.
    public void ResetPlayerDecision(){

        //if the player has decided not to take cards then he cannot change his mind at the next draw
        if(stateMachine.currentState != rejectCardState && stateMachine.currentState != loseState){
            decidedWhatToDo = false;
            askingForCard = false;
            receivedCard = false;
            waitingForNextTurn = false;
            stateMachine.ChangeState(decisionState);
        }else{
            waitingForNextTurn = false;
            FinishTurn();
        }
    }

    //reset the player for the start of a new game
    public void ResetPlayerToStart(){
        decidedWhatToDo = false;
        askingForCard = false;
        receivedCard = false;
        waitingForNextTurn = false;
        lost = false;
        rejectCards = false;
        numberOfCards = 0;
        cardSum = 0;
        cardObjectList = new List<CardObject>();
        currentXOffset = 0;
        cardSumText.text = cardSum.ToString();
        stateMachine.ChangeState(waitingForCardState);
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

        //remove the velocity from the rigidbody
        cardObject.SetVelocity(Vector3.zero);

        //should move the card in the direction of its local axis
        //Send card on the table in front of player faced up, (could move it slowly there with a transform.translate and removing rb forces)
        CardGameObject.transform.position = new Vector3 (cardPosition.position.x, cardPosition.position.y, cardPosition.position.z);
        //Change local position so that the card moves on the x axis with respect to the card rotation
        CardGameObject.transform.localPosition += transform.right * currentXOffset;
        CardGameObject.transform.rotation = cardPosition.rotation;

        UpdateCardSum(cardObject.GetValue());

        //deActivate the card so that the player can't pick it up again
        cardObject.RemoveInteraction();

        receivedCard = true;
        askingForCard = false;
        currentXOffset = currentXOffset + cardXOffset;
        FinishTurn();
    }

    public void UpdateCardSum(int value){
        //check if it's an ace
        if(value == 11){
            numberOfAces ++;
        }

        numberOfCards++;

        cardSum = cardSum + value;

        CheckIfLost();

        cardSumText.text = cardSum.ToString();
    }

    //Check if the player lost, before doing so check if he has aces, if he does then check if player loses even if the ace value is 1
    public void CheckIfLost(){
        if(cardSum>21){
            if(CheckAces()){
                cardSum = cardSum - 10; //give the ace a value of 1
                numberOfAces --;
                CheckIfLost(); //Check again if lost anyway, I think it's impossible though
            }else{
                lost = true;
                return;
            }
        }
    }

    public bool CheckAces(){
        if(numberOfAces > 0){
            return true;
        }else{
            return false;
        }
    }

    //function called by the card if dragged onto the player
    public void AssignCard(GameObject cardGO){
        if(CheckAssignCard()){
            AddCardToPlayerHand(cardGO);
            //cardObject.RemoveInteraction();
        }
    }

    public void OnMouseOver(){
        if(showStats){
            statsCanvas.SetActive(true);
        }
    }

    public void OnMouseExit(){
        statsCanvas.SetActive(false);
    }

    public void SetName(string name){
        playerName = name;
        statsPlayerName.text = playerName;
    }

    public void SetPercentage(int percentage){
        askingPercentage = percentage;
        SetPercentageCanvas(percentage);
    }

    public string GetName(){
        return playerName;
    }

    #region Canvas Stats

    public void SetPercentageCanvas(int percentage){
        statsDrawPercentage.text = percentage.ToString() + "0%";
    }

    public void SetCurrentStateCanvas(string canvasState){
        statsCurrentState.text = canvasState;
    }

    #endregion

    #region Sounds

    //Play the sound one time only
    public void PlayOneShotSound(AudioClip audio){
        audioSource.loop = false;
        audioSource.PlayOneShot(audio);
    }

    //Play sound in loop
    public void PlaySound(AudioClip audio){
        audioSource.loop = true;
        audioSource.clip = audio;
        audioSource.Play();
    }

    //stop sound
    public void StopSound(){
        audioSource.Stop();
    }

    #endregion

    #region ChangeState region
    //change the player to win state, it's always called from the player manager
    public void ChangeToWinState(){
        stateMachine.ChangeState(winState);
    }

    //only called from PlayerManager
    public void ChangeToLostState(){
        stateMachine.ChangeState(loseState);
    }

    #endregion

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
