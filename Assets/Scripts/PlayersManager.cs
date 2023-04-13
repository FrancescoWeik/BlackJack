using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    public int numberOfPlayers;
    public int maxNumberOfPlayers;

    //public GameObject playerPrefab;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<Vector3> possiblePlayerPositions;

    public int numberOfPlayersWaitingTurn;

    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        numberOfPlayersWaitingTurn = 0;
        //TODO instantiate all players
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
        for(int i=0; i<players.Count; i++){
            players[i].ResetPlayerDecision();
        }
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

            GameManager.Instance.StartPlayerTurn();
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
            //Debug.Log(players[i].lost);
            if(!players[i].lost){
                return false;
            }
        }
        return true;
    }

    //Check which players won based on their card value. Receives the dealer card sum value as a parameter.
    public void CheckPlayerWin(int dealerValue){
        List<Player> playersWhoWon = new List<Player>();

        for(int i = 0; i < players.Count; i++){
            if((players[i].GetCardSum() > dealerValue || dealerValue>21) && !players[i].lost){
                players[i].ChangeToWinState();
                playersWhoWon.Add(players[i]);
            }else{
                players[i].ChangeToLostState();
            }
        }

        if(playersWhoWon.Count >0 ){
            GameManager.Instance.PlayersWon(playersWhoWon);
        }
    }
}
