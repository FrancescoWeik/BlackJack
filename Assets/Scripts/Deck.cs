using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*I could have removed the deck mesh but then the cards would all have been flat on the table
For that reason I decided to keep the deck mesh and structured the deck in a different way so that it creates the cards on the top of it every time it's needed*/

public class Deck : MonoBehaviour
{

    private List<Card> cards;
    [SerializeField] private List<GameObject> cardsPrefabs; //card prefabs, ordered

    [SerializeField] private float drawCardOffsetY = 1.35f; //offset needed to position the cards in the right position

    [SerializeField] private LayerMask whatIsCard;
    [SerializeField] private GameObject simpleCard; //prefab of an empty car

    [SerializeField] private GameObject fullDeck; //the full deck mesh
    [SerializeField] private GameObject halfDeck; //the half deck mesh
    [SerializeField] private GameObject shuffleDeckMesh; //the gameobject used for the shuffle animation
    [SerializeField] private int halfDeckThreshold; //number of cards to have to change mesh to half deck
    private bool shuffling; //while shuffling you can't draw

    public int numberOfChilds; //the number of the child of the deck gameobject, it's useful to know in order to perform animations and shuffling.

    private void Start(){
        InitializeDeck();
        shuffling = false;
    }

    private void Update(){
        if(!CheckCard()){
            if(!shuffling){
                CreateEmptyCard();
            }
        }else{
            //Do nothing
        }
    }

    //Initialize an ordered deck with the cards, assigning values, suits and a prefab used for the materials.
    void InitializeDeck(){
        List<Card> orderedDeck = new List<Card>();
        for(int j=0; j < 4; j++){
            for(int i=0; i<13; i++){
                orderedDeck.Add(new Card(j, i + 1, this.cardsPrefabs[(13*j) + i].GetComponent<Renderer>().material));
            }
        }

        cards = orderedDeck;
        //printDeck();
    }

    //randomly shuffle all the cards inside the deck
    public void Shuffle(){

        //if the deck contains no cards you can't shuffle it
        if(cards.Count == 0){
            return;
        }

        //Set the meshes and the last card to false so that you can play the animation
        halfDeck.SetActive(false);
        fullDeck.SetActive(false);
        transform.GetChild(transform.childCount -1).gameObject.SetActive(false);

        //play the deck animation
        shuffling = true;
        shuffleDeckMesh.SetActive(true);

        //put the cards randomly in the deck
        for(int i=0; i<cards.Count; i++){
            Card temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
        //printDeck();
    }

    //shoot a raycast on the deck normal to check whether or not there's a card, if there isn't then create one.
    private bool CheckCard(){
        Ray ray;
        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.up);

        //Debug.DrawRay(transform.position,  Vector3.up, Color.blue);

        return Physics.Raycast(ray, out hit, 2f, whatIsCard);
    }

    //Create an empty card on top of the deck, it is later initialized when clicked
    private void CreateEmptyCard(){
        if(cards.Count > 0){
            Vector3 instatiatePosition = new Vector3(transform.position.x, transform.position.y + drawCardOffsetY, transform.position.z);
            Quaternion simpleCardRotation = new Quaternion(0, 0, 180, 1);
            Instantiate(simpleCard, instatiatePosition, simpleCardRotation, transform);
            
            //if we havee only halfDeckThreshold cards then show a smaller deck
            if(cards.Count < halfDeckThreshold){
                //change to half deck mesh
                halfDeck.SetActive(true);
                fullDeck.SetActive(false);
            }
        }
        else{
            //remove deck because there are no cards left
            fullDeck.SetActive(false);
            halfDeck.SetActive(false);
        }
    }

    //Initialize the top card of the deck. Called from the card
    public void InitializeCard(CardObject cardObject){
        Card card = cards[cards.Count -1];
        cards.RemoveAt(cards.Count -1);
        cardObject.SetValueAndSuit(card.value, card.suitChar);
        cardObject.SetMaterial(card.material);

    }

    //function called at the end of the round, put all the cards that have been played at the bottom of the deck
    public void PutCardsAtBottom(){
        /*
            One way to do that is to search all the children of the deck (eexcept the last one) and add their card values to the deck.
            This can be done only if the cards played are all children of the deck as in this case. 
        */

        //Show the full deck to the user
        fullDeck.SetActive(true);
        halfDeck.SetActive(false);

        //Check if I have no card in the deck
        int childListLength;
        if(cards.Count == 0){
            //last card havee been played, remove it too
            childListLength = transform.childCount;
            Debug.Log("All Cards");
        }else{
            //childCount -1 because the last card has not been played and so it has to stay on the top!
            childListLength = transform.childCount -1;
            Debug.Log("All Cards minus the top one");
        }

        //i = numberOfChilds because the deck has also other children besides the cards, so I have to ignore those.
        //could have also checked if the child had the CardObject component instead
        for(int i = numberOfChilds; i < childListLength; i++){
            
            int bottomCardValue = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetValue();
            char bottomCardSuit = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetSuit();
            Material bottomCardMaterial = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetMaterial();
            Card bottomCard = new Card(bottomCardSuit, bottomCardValue, bottomCardMaterial);

            //insert card at the bottom of the list
            cards.Insert(0,bottomCard);

            Destroy(transform.GetChild(i).gameObject);
        }
        

    }

    //called by the DeckShuffleAnimation, set the fulldeck mesh and the last card to active
    public void FinishShuffleAnim(){
        shuffleDeckMesh.SetActive(false);
        halfDeck.SetActive(false);
        fullDeck.SetActive(true);
        transform.GetChild(transform.childCount -1).gameObject.SetActive(true);
        shuffling = false;
    }

    void printDeck(){
        for(int i=0; i<cards.Count; i++){
            cards[i].Print();
        }
        Debug.Log(cards.Count);
    }   

}
