using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Card class used to initialize the CardObject later when clicked.
public class Card
{
    public char suitChar;
    public int value;
    public Material material;

    public Card(int suit, int value, Material cardMesh){
        switch(suit){
            case 0: suitChar = '♦'; break;
            case 1: suitChar = '♥'; break;
            case 2: suitChar = '♣'; break;
            case 3: suitChar = '♠'; break;
            default: break;
        }

        //in blackjack the figures value is 10.
        switch(value){
            case 1: this.value = 11; break; //the ace value is 11 unless the player busts.
            case 13:
            case 12:
            case 11: this.value = 10; break;
            default: this.value = value; break;
        }

        //this.value = value;
        this.material = cardMesh;
    }

    public void Print(){
        Debug.Log("card is " + material + " value: " + value + " suit: " + suitChar);
    }
}
