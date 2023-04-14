using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DealerHand dealer;

    public bool isPlayerTurn;

    public GameObject endRoundCanvas;
    public Text winnerText;

    public Deck deck; //need the deck to be able to handle the deck reset at end of game

    // Start is called before the first frame update
    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        
        isPlayerTurn = true;
    }

    // Update is called once per frame
    /*void Update()
    {
        if(isPlayerTurn){
            //bool finishedDeciding = PlayersManager.Instance.CheckAllPlayerFinished();
            //if(finishedDeciding){
                //StartDealerTurn();
            //}
        }else{
            //dealer turn
        }
    }*/


    public void StartDealerTurn(){
        isPlayerTurn = false;
        //Debug.Log("Dealer Turn");

        //if all player exceeds 21 then you the dealer doesn't draw and wins
        if(PlayersManager.Instance.CheckAllPlayersLost()){
            /*
                not sure about this one because I can't find rules regarding this special case.
                if all players exceed 21 does the dealer still have to draw?
            */
            EndGame();
        }else{
             //if dealer cannot draw then go back to player turn.
            if(!dealer.CheckCanDraw()){
                if(PlayersManager.Instance.CheckPlayersCanPlay()){
                    PlayersManager.Instance.StartPlayerTurn();
                }else{
                    EndGame();
                }
            }
        }
    }

    public void StartPlayerTurn(){
        isPlayerTurn = true;
    }

    //handle logic for when someone won the game
    public void EndGame(){
        if(PlayersManager.Instance.CheckAllPlayersLost()){
            //Dealer Won
            DealerWon();

        }else{
            //Players won, which ones?
            //Debug.Log("Player Might have won");
            int dealerValue = dealer.GetDealerCardSum();
            PlayersManager.Instance.CheckPlayerWin(dealerValue);
        }
    }

    //function handling the case where the dealer won, it displays the canvas
    public void DealerWon(){
        winnerText.text = "Dealer Won";
        endRoundCanvas.SetActive(true);
    }

    //function handling the case where the players won, it displays the canvas
    public void PlayersWon(List<Player> playerList){
        string textWinner = "Winners:\n";  
        for(int i = 0; i < playerList.Count; i++){
            textWinner = textWinner + "- " + playerList[i].ToString() + "\n";
        }
        winnerText.text =  textWinner;
        endRoundCanvas.SetActive(true);
    }

    //immediately start a new round when one finishes.
    public void StartNextRound(){
        Debug.Log("Starting next round");

        //TODO Play deck shuffle animation

        //Get All cards and put them at the bottom of the deck
        deck.PutCardsAtBottom();


        //this part will later be called by the deck animation
        dealer.ResetToStart();
        PlayersManager.Instance.ResetPlayersToStart();
    }

    //Function called using the ui, it starts the game
    public void StartGame(int numberOfPlayers){
        PlayersManager.Instance.InstantiateAllPlayers(numberOfPlayers);
    }
    
    public void QuitGame(){
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
