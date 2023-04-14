using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinState : PlayerState
{
    public PlayerWinState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        PlayClappingSound();
    }

    public override void Exit()
    {
        base.Exit();

        player.StopSound();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //just play the win animation

    }

    private void PlayClappingSound(){
        //play player clap hands sound
        player.PlaySound(playerData.winSound);
    }
}