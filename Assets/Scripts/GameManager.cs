using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DealerHand dealer;

    public bool isPlayerTurn;

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
        Debug.Log("Dealer Turn");

        //if all player exceeds 21 then you the dealer doesn't draw and wins
        if(PlayersManager.Instance.CheckAllPlayersLost()){
            /*
                not sure about this one because I can't find rules regarding this special case.
                if all players exceed 21 does the dealer still have to draw?
                TODO ask to Lucas
            */
            EndGame();
        }else{
             //if dealer cannot draw then go back to player turn.
            if(!dealer.CheckCanDraw()){
                PlayersManager.Instance.StartPlayerTurn();
            }
        }
    }

    public void StartPlayerTurn(){
        isPlayerTurn = true;
    }

    //handle logic for when someone won the game
    public void EndGame(){
        Debug.Log("end game");
        if(PlayersManager.Instance.CheckAllPlayersLost()){
            //Dealer Won
        }else{
            //Players won, which ones?
        }
    }
}
