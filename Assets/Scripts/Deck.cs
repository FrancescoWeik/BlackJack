using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*I could have removed the deck mesh but then the cards would all have been flat on the table
For that reason I decided to keep the deck mesh and structured the deck in a different way so that it creates the cards on the top of it every time it's needed*/

public class Deck : MonoBehaviour
{

    private List<Card> cards;
    [SerializeField] public List<GameObject> cardsPrefabs; //card prefabs, ordered

    private void Start(){
        InitializeDeck();
    }

    //Initialize an ordered deck with the cards, assigning values, suits and a prefab used for the materials.
    void InitializeDeck(){
        List<Card> orderedDeck = new List<Card>();
        for(int j=0; j < 4; j++){
            for(int i=0; i<13; i++){
                orderedDeck.Add(new Card(j, i + 1, this.cardsPrefabs[(13*j) + i]));
            }
        }

        cards = orderedDeck;
        //printDeck();
    }

    void printDeck(){
        for(int i=0; i<cards.Count; i++){
            cards[i].Print();
        }
        Debug.Log(cards.Count);
    }   

    //randomly shuffle all the cards inside the deck
    public void Shuffle(){
        /**
            TODO start an animation while shuffling the deck
        */
        
        for(int i=0; i<cards.Count; i++){
            Card temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
        printDeck();
    }

    //Initialize the top card of the deck.
    public void InitializeCard(CardObject cardObject){
        Card card = cards[cards.Count -1];
        cards.RemoveAt(cards.Count -1);
        cardObject.SetValueAndSuit(card.value, card.suitChar);
        cardObject.SetMaterial(card.material);

    }

}
