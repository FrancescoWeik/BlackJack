using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    public int numberOfPlayers;
    public int maxNumberOfPlayers;

    private List<Player> players;
    [SerializeField] private List<Vector3> possiblePlayerPositions;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance!=null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks if all the players have finished deciding what to do, since it takes so little I'll add a decision timer
    public bool CheckAllPlayerFinished(){
        for(int i=0; i<players.Count; i++){
            if(!players[i].decidedWhatToDo){
                return false;
            }
        }
        return true;
    }

    public void ResetPlayersDecisions(){
        for(int i=0; i<players.Count; i++){
            players[i].ResetPlayerDecision();
        }
    }
}
