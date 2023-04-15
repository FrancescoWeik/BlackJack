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
    //public GameObject menuScreen;
    public GameObject pauseMenuScreen;
    public GameObject pauseButton;

    public Deck deck; //need the deck to be able to handle the deck reset at end of game
    public GameObject arrow; //arrow that points at the dealer field

    public PlayerNumberScriptable playerNumberScriptable;

    // Start is called before the first frame update
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
            //Time.timeScale = 0;
            PauseGame();
        }
    }

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
            }else{
                //dealer turn
                Debug.Log("Dealer turn");

                //activate arrow
                arrow.SetActive(true);
            }
        }
    }

    public void StartPlayerTurn(){
        isPlayerTurn = true;

        //disable arrow
        arrow.SetActive(false);

        //start the player turn
        PlayersManager.Instance.StartPlayerTurn();
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
        //Debug.Log("Quit Game");
        //Application.Quit();
        Time.timeScale = 1f;
        LevelManager.Instance.LoadScene("MenuScene"); 
    }

    public void PauseGame(){
        //open pause game menu\
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
