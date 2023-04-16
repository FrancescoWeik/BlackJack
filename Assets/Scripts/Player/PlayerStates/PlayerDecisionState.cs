using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecisionState : PlayerState
{
    int number;
    public PlayerDecisionState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData,  animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        number = Random.Range(0,10);
        player.SetCurrentStateCanvas("Decision state");
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

        //if the playeer score is less than the dealer and the dealer cannot draw anymore, the player has to draw otherwise he will lose for sure.
        if(player.GetCardSum() < player.dealerHand.GetDealerShownCardSum() && (player.dealerHand.GetDealerCardSum()<21 && player.dealerHand.GetDealerShownCardSum()>17)){
            stateMachine.ChangeState(player.waitingForCardState);
        }
        else if(number > player.askingPercentage || player.ExceedMaxAskingPoints()){
            //Do Not Ask For Card
            player.askingForCard = false;
            player.decidedWhatToDo = true;

            //check if sum of cards is bigger than 21, if it is then lost
            if(player.GetCardSum() > player.maxBlackJack){
                stateMachine.ChangeState(player.loseState);
            }else{  
                if(player.numberOfCards>=2){
                    stateMachine.ChangeState(player.rejectCardState);
                }
                //player must draw if he has less than 2 cards.
                else{
                    stateMachine.ChangeState(player.waitingForCardState);
                }
            }
        }else{
            //check if dealer has 17 or more, if player already has more than the dealer then do not ask cards
            if(player.dealerHand.GetDealerShownCardSum() >= 17 && player.GetCardSum() > 17){
                player.decidedWhatToDo = false;
                player.askingForCard = false;
                stateMachine.ChangeState(player.rejectCardState);
            }else{
                //if want a card then change to wait card state
                player.decidedWhatToDo = true;
                stateMachine.ChangeState(player.waitingForCardState);
            }
        }
    }
}