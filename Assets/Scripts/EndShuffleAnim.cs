using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndShuffleAnim : MonoBehaviour
{
    public Deck deck;

    //called when deck finishes the shuffle animation
    public void FinishAnim(){
        deck.FinishShuffleAnim();
    }
}
