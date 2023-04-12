using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecisionState : PlayerState
{
    int number;
    public PlayerDecisionState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        number = Random.Range(0,10);
    }

    public override void Exit()
    {
        base.Exit();

        player.decidedWhatToDo = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        /*if(player.numberOfCards<2){
            if(player.GetCardSum() > player.maxBlackJack){
                stateMachine.ChangeState(player.loseState);
            }else{
                //must ask for card
                player.decidedWhatToDo = true;
                stateMachine.ChangeState(player.waitingForCardState);
            }
        }*/

        if(number > player.askingPercentage || player.ExceedMaxAskingPoints()){
            //Do Not Ask For Card
            player.askingForCard = false;
            player.decidedWhatToDo = true;

            //check if sum of cards is bigger than 21, if it is then lost
            if(player.GetCardSum() > player.maxBlackJack){
                stateMachine.ChangeState(player.loseState);
            }else{  
                if(player.numberOfCards>=2){
                    stateMachine.ChangeState(player.rejectCardState);
                }else{
                    stateMachine.ChangeState(player.waitingForCardState);
                }
            }
        }else{
            //if want a card then change to wait card state
            player.decidedWhatToDo = true;
            stateMachine.ChangeState(player.waitingForCardState);
        }
    }
}