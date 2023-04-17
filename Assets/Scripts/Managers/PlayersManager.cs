using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    public int numberOfPlayers;
    public int maxNumberOfPlayers;

    public int minDrawPercentage = 3;
    public int maxDrawPercentage = 9;

    public GameObject playerPrefab;
    [SerializeField] private List<string> playerNamesList;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<Transform> possiblePlayerPositions;

    public int numberOfPlayersWaitingTurn;

    [SerializeField] private GameObject playerTurnCanvas;

    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        numberOfPlayersWaitingTurn = 0;
    }

    //instantiate all players and add them into the list of playing players
    public void InstantiateAllPlayers(int numberOfPlayers){
        players = new List<Player>();
        this.numberOfPlayers = numberOfPlayers;
        for(int i = 0; i < numberOfPlayers; i++){
            GameObject singlePlayerGO = Instantiate(playerPrefab, possiblePlayerPositions[i].position, possiblePlayerPositions[i].rotation);

            //assign asking for a card percentage
            Player singlePlayer = singlePlayerGO.GetComponent<Player>();
            singlePlayer.SetPercentage(Random.Range(minDrawPercentage,maxDrawPercentage));

            //assign player names
            int randomPlayerName = Random.Range(0, playerNamesList.Count);
            singlePlayer.SetName(playerNamesList[randomPlayerName]);
            //singlePlayer.playerName = playerNamesList[randomPlayerName];

            players.Add(singlePlayer);
        }
    }

    //Remove all the existing players from the scene
    public void RemoveExistingPlayers(){
        Debug.Log(numberOfPlayers);
        for(int i=0; i < numberOfPlayers; i++){
            Destroy(players[i].gameObject);
        }
    }

    //Checks if all the players have finished deciding what to do, since it takes so little I can add a decision timer to make the dealer wait
    public bool CheckAllPlayerFinished(){
        for(int i=0; i<players.Count; i++){
            if(!players[i].decidedWhatToDo){
                return false;
            }
        }
        return true;
    }

    //Reset all players decision when the player turn starts.
    public void ResetPlayersDecisions(){

        //show the user that players are choosing what to do
        //playerTurnCanvas.SetActive(true);

        for(int i=0; i<players.Count; i++){
            players[i].ResetPlayerDecision();
        }
    }

    //Reset all players to the parameters they have at the start of the game
    public void ResetPlayersToStart(){
        for(int i=0; i<players.Count; i++){
            players[i].ResetPlayerToStart();
        }

        StartPlayerTurn();
    }

    //add +1 to the number of player that finished the turn, if all did then it's the dealer turn!
    public void AddWaitingForTurn(){
        numberOfPlayersWaitingTurn ++;
        if(numberOfPlayersWaitingTurn >= players.Count){
            //All the players received the card if the wanted one, now it's the dealer turn to draw a card.
            GameManager.Instance.StartDealerTurn();
        }
    }

    public void StartPlayerTurn(){
        
        //Check if all player don-t want cards or lost, if all of them don't want cards then end game and checks who win
        if(CheckPlayersCanPlay()){
            numberOfPlayersWaitingTurn = 0;

            //reset the player decisions at the start of their turn, resetting the animations too if they haven't lost or might card
            ResetPlayersDecisions();

        }else{
            GameManager.Instance.StartDealerTurn();
            //GameManager.Instance.EndGame();
        }
    }

    //if a player still hasn't lost or might still want cards then change to player turn.
    public bool CheckPlayersCanPlay(){
        for(int i = 0; i < players.Count; i++){
            if(!players[i].lost && !players[i].rejectCards){
                return true;
            }
        }
        return false;
    }

    //if all players lost return true
    public bool CheckAllPlayersLost(){
        for(int i = 0; i < players.Count; i++){
            if(!players[i].lost){
                return false;
            }
        }
        return true;
    }

    public bool CheckPlayersBlackJack(){
        for(int i = 0; i < players.Count; i++){
            if(players[i].blackjack){
                return true;
            }
        }
        return false;
    }

    //find the player that has a blackjack at first 2 card hand
    public void CheckPlayersBlackJackWinner(int dealerValue){
        List<string> playersBlackJackWin = new List<string>();
        bool draw = false;
        //check if player has a blackjack
        for(int i = 0; i < players.Count; i++){
            if(players[i].blackjack){
                playersBlackJackWin.Add(players[i].GetName());
                //if dealer has blackjack too then it's a draw, set to idle, otherwise set to win
                if(dealerValue == 21){
                    players[i].ChangeToIdleState();
                    draw = true;
                }else{
                    players[i].ChangeToWinState();
                }
            }
        }

        if(playersBlackJackWin.Count!=0){
            GameManager.Instance.PlayersWon(playersBlackJackWin, draw, true);
            return;
        }
    }

    //Check which players won based on their card value. Receives the dealer card sum value as a parameter.
    public void CheckPlayerWin(int dealerValue, bool isBlackjack){
        if(isBlackjack){
            CheckPlayersBlackJackWinner(dealerValue);
            return;
        }

        //If it is not a blackjack and 
        List<string> playersWhoWon = new List<string>();
        bool draw = false;

        for(int i = 0; i < players.Count; i++){
            if(players[i].GetCardSum() == dealerValue){
                //draw
                players[i].ChangeToWinState();
                playersWhoWon.Add(players[i].GetName());
                draw = true;
            }
            else if((players[i].GetCardSum() > dealerValue || dealerValue>21) && !players[i].lost){
                //player win
                players[i].ChangeToWinState();
                playersWhoWon.Add(players[i].GetName());
            }
            else{
                //player lose
                players[i].ChangeToLostState();
            }
        }

        if(playersWhoWon.Count > 0 ){
            GameManager.Instance.PlayersWon(playersWhoWon,draw, false);
        }else{
            GameManager.Instance.DealerWon();
        }
    }

    //activate all the player canvases showing their stats. They will be visible only on mouse hover
    public void ShowPlayerStats(){
        for(int i=0; i < numberOfPlayers; i++){
            players[i].showStats = true;
        }
    }

    //remove the player canvases showing their stats
    public void RemovePlayerStats(){
        for(int i=0; i < numberOfPlayers; i++){
            players[i].showStats = false;
        }
    }
}
