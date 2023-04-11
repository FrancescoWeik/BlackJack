using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    List<CardObject> cardObjectList; //list of the cards that the player has while playing
    public int stopAskingAtPoints = 20; //above 20 points the player won't ask for more cards

    public int askingPercentage = 7; //percentage out of ten, so with 7 is 70%

    public bool askingForCard;
    public bool decidedWhatToDo;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        askingForCard = true;
        decidedWhatToDo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isPlayerTurn){
            int number = Random.Range(0,10);
            if(number > askingPercentage){
                //Do Not Ask For Card
                askingForCard = false;
            }else{
                //Ask For A Card
                askingForCard = true;
                /**
                TODO play animation "waiting for card"
                */
            }
            decidedWhatToDo = true;
        }
    }

    public void ResetPlayerDecision(){
        decidedWhatToDo = false;
        askingForCard = false;
    }
}
