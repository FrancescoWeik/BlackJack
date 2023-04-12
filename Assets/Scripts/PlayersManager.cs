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

    void Update()
    {
        
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

            /*
            //should check if everyone is saying no or has lost before doing so

            //reset the player decisions at the start of their turn, resetting the animations too
            ResetPlayersDecisions();

            GameManager.Instance.StartPlayerTurn();

            numberOfPlayersWaitingTurn = 0;
            */
        }
    }

    public void StartPlayerTurn(){
        
        //should check if everyone is saying no or has lost before doing so

        //reset the player decisions at the start of their turn, resetting the animations too
        ResetPlayersDecisions();

        GameManager.Instance.StartPlayerTurn();

        numberOfPlayersWaitingTurn = 0;
        
    }

    /*public void SetPlayerToDecideState(){
        for(int i=0; i<players.Count; i++){
            players[i].ResetPlayerDecision();
        }
    }*/
}
