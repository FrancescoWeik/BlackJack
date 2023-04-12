using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLostState : PlayerState
{
    public PlayerLostState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //just play the lost animation

    }
}
