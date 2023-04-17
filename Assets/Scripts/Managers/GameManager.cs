using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerTurn;

    [Header("UI")]
    public GameObject endRoundCanvas;
    public Text winnerText;
    //public GameObject menuScreen;
    public GameObject pauseMenuScreen;
    public GameObject pauseButton;

    [Header("Dealere And Deck")]
    public Deck deck; //need the deck to be able to handle the deck reset at end of game
    public GameObject arrow; //arrow that points at the dealer field
    public DealerHand dealer;

    [Header("Scriptable")]
    public PlayerNumberScriptable playerNumberScriptable; //needed to know the number of player to put in the scene

    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);
        
        isPlayerTurn = true;
        StartGame(playerNumberScriptable.numberOfPlayer);
    }

    public void Update(){
        if(Input.GetKey(KeyCode.Escape)){
            //Open menu
            PauseGame();
        }
    }

    public void StartDealerTurn(){
        isPlayerTurn = false;

        //if all player exceeds 21 then you the dealer doesn't draw and wins
        if(PlayersManager.Instance.CheckAllPlayersLost()){
            EndGame();
        }else{
             //if dealer cannot draw then go back to player turn, otherwise the game ends
            if(!dealer.CheckCanDraw()){
                if(PlayersManager.Instance.CheckPlayersCanPlay()){
                    PlayersManager.Instance.StartPlayerTurn();
                }else{
                    EndGame();
                }
            }else{
                //dealer turn
                Debug.Log("Dealer turn");

                //activate arrow pointing to where dealer should put the cards
                arrow.SetActive(true);
            }
        }
    }

    public void StartPlayerTurn(){
        //if a player has a blackjack then the game ends, otherwise continue
        if(PlayersManager.Instance.CheckPlayersBlackJack()){
            EndGame();
        }else{
            isPlayerTurn = true;

            //disable arrow
            arrow.SetActive(false);

            //start the player turn
            PlayersManager.Instance.StartPlayerTurn();
        }
    }

    //handle logic for when someone won the game
    public void EndGame(){
        //Flip the second card of the dealer at the end of the game if it isn't already flipped
        dealer.FlipCard();

        //if a player has a blackjack then check which players won.
        if(PlayersManager.Instance.CheckPlayersBlackJack()){
            int dealerValue = dealer.GetDealerCardSum();
            PlayersManager.Instance.CheckPlayerWin(dealerValue, true);
        }

        //otherwise check if all player lost
        else if(PlayersManager.Instance.CheckAllPlayersLost()){
            //Dealer Won
            DealerWon();

        }else{
            //Players won, check which players won
            int dealerValue = dealer.GetDealerCardSum();
            PlayersManager.Instance.CheckPlayerWin(dealerValue, false);
        }
    }

    //function handling the case where the dealer won, it displays the canvas
    public void DealerWon(){
        winnerText.text = "Dealer Won";
        endRoundCanvas.SetActive(true);
    }

    /*
        function handling the case where the players won, it displays the canvas. 
        Requires the list of the player who won, a boolean telling the game if it was a draw and a boolean checking for blackjacks
    */
    public void PlayersWon(List<string> playerList, bool isDraw, bool isBlackJack){
        //Flip the second card of the dealer at the end of the game if it isn't already flipped
        dealer.FlipCard();

        string textWinner = "";

        if(isBlackJack){
            textWinner = "BLACKJACK! \n";
        }

       textWinner = textWinner + "Winners:\n";  

        //dealer and player win if draw
        if(isDraw){
            textWinner =  "It's a draw! Winners: \n" + "- Dealer" + "\n";
        }

        for(int i = 0; i < playerList.Count; i++){
            textWinner = textWinner + "- " + playerList[i].ToString() + "\n";
        }
        winnerText.text =  textWinner;
        endRoundCanvas.SetActive(true);
    }

    //immediately start a new round when one finishes.
    public void StartNextRound(){
        //You can add an animation here at the start of each round. I decided not to so that the game feels faster

        //Get All cards and put them at the bottom of the deck
        deck.PutCardsAtBottom();

        //disable arrow
        arrow.SetActive(false);
        
        //this part will later be called by the deck animation
        dealer.ResetToStart();
        PlayersManager.Instance.ResetPlayersToStart();
        isPlayerTurn = true;
    }

    //Function called using the ui, it starts the game
    public void StartGame(int numberOfPlayers){
        PlayersManager.Instance.RemoveExistingPlayers();
        PlayersManager.Instance.InstantiateAllPlayers(numberOfPlayers);
        isPlayerTurn = true;
        pauseButton.SetActive(true);
    }
    
    public void QuitToMenu(){
        Time.timeScale = 1f;
        LevelManager.Instance.LoadScene("MenuScene"); 
    }

    public void PauseGame(){
        //open pause game menu
        pauseButton.SetActive(false);
        pauseMenuScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void UnPauseGame(){
        //close pause game menu
        pauseMenuScreen.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }
}
