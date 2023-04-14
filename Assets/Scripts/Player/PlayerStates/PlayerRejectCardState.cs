using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRejectCardState : PlayerState
{
    public PlayerRejectCardState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.rejectCards = true;

        //if a player rejects card then he has finished his turn and is waiting for the next turn.
        player.FinishTurn();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //just wait for the end of the turn so do nothing. 
    }
}