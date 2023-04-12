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

    int numberOfPlayersWaitingTurn;

    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

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
            //GameManager.Instance.StartDealerTurn();
            ResetPlayersDecisions();
            GameManager.Instance.StartPlayerTurn();
        }
    }
}
