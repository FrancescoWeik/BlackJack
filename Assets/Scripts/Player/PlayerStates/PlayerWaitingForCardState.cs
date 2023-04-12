using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingForCardState : PlayerState
{   

    public PlayerWaitingForCardState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
            stateMachine.ChangeState(player.idleState);
        }else{
            //wait for card...
        }
    }
}