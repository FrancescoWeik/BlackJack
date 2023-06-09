using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName){
        this.player = player;
        this.playerData = playerData;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter(){
        player.anim.SetBool(animBoolName, true);
        isAnimationFinished = false;
    }

    public virtual void Exit(){
         player.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate(){ //every frame

    }

    public virtual void AnimationFinishTrigger(){
        isAnimationFinished = true;
    }

}
