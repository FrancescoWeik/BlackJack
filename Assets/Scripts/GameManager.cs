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
    void Update()
    {
        if(isPlayerTurn){
            //bool finishedDeciding = PlayersManager.Instance.CheckAllPlayerFinished();
            //if(finishedDeciding){
                //StartDealerTurn();
            //}
        }else{
            //dealer turn
        }
    }

    public void StartDealerTurn(){
        isPlayerTurn = false;
        Debug.Log("Dealer Turn");

        //if dealer cannot draw then go back to player turn.
        if(!dealer.CheckCanDraw()){
            //Check if all player don-t want cards, if all of them don't want cards then end game and checks who win

            PlayersManager.Instance.StartPlayerTurn();
        }
    }

    public void StartPlayerTurn(){
        isPlayerTurn = true;
    }
}
