using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    public int numberOfPlayers;
    public int maxNumberOfPlayers;

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

            //assign asking percentage
            Player singlePlayer = singlePlayerGO.GetComponent<Player>();
            singlePlayer.SetPercentage(Random.Range(3,9));
            //singlePlayer.askingPercentage = Random.Range(3,9);

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

    //Check which players won based on their card value. Receives the dealer card sum value as a parameter.
    public void CheckPlayerWin(int dealerValue){
        List<string> playersWhoWon = new List<string>();

        for(int i = 0; i < players.Count; i++){
            //TODO check case when dealer has same points as player, that case is a draw... For now it's a win for the player

            if((players[i].GetCardSum() >= dealerValue || dealerValue>21) && !players[i].lost){
                players[i].ChangeToWinState();
                playersWhoWon.Add(players[i].GetName());
            }else{
                players[i].ChangeToLostState();
            }
        }

        if(playersWhoWon.Count > 0 ){
            GameManager.Instance.PlayersWon(playersWhoWon);
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
