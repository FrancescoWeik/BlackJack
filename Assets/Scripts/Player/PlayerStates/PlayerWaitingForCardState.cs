using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingForCardState : PlayerState
{   

    public PlayerWaitingForCardState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.askingForCard = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate(); 

        //Debug.Log("waiting for a card");

        if(player.CheckReceivedCard()){
            
            //check if sum exceeds, it it does then change to lose
            if(player.GetCardSum() > player.maxBlackJack){
                stateMachine.ChangeState(player.loseState);
            }else{
                stateMachine.ChangeState(player.idleState);
            }
        }else{
            //wait for card...
        }
    }
}