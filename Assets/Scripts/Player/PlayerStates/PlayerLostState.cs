using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLostState : PlayerState
{
    public PlayerLostState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.SetCurrentStateCanvas("Lost state"); 

        player.lost = true;
        //player.PlayOneShotSound(playerData.lostSound); if you want a "screm" sound when losing remove comment
        player.FinishTurn();
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
