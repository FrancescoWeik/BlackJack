using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*I could have removed the deck mesh but then the cards would all have been flat on the table
For that reason I decided to keep the deck mesh and structured the deck in a different way so that it creates the cards on the top of it every time it's needed*/

public class Deck : MonoBehaviour
{

    private List<Card> cards;
    [SerializeField] private List<GameObject> cardsPrefabs; //card prefabs, ordered

    [SerializeField] private float drawCardOffsetY = 1.35f;

    [SerializeField] private LayerMask whatIsCard;
    [SerializeField] private GameObject simpleCard; //prefab of an empty car

    [SerializeField] private GameObject fullDeck; //the full deck mesh
    [SerializeField] private GameObject halfDeck; //the half deck mesh
    [SerializeField] private GameObject shuffleDeckMesh; //the gameobject used for the shuffle animation
    [SerializeField] private int halfDeckThreshold; //number of cards to have to change mesh to half deck
    private bool shuffling;

    public int numberOfChilds; //the number of the child of the deck gameobject, it's useful to know in order to perform animations and shuffling.

    //private Collider col;

    private void Start(){
        //meshRenderer = GetComponent<MeshRenderer>();
        //col = GetComponent<Collider>();

        InitializeDeck();
        shuffling = false;
    }

    private void Update(){
        if(!CheckCard()){
            //Debug.Log("Card missing");
            if(!shuffling){
                CreateEmptyCard();
            }
        }else{
            //Debug.Log("Card on deck");
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

        //Set the meshes and the last card to false
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

    //shoot a raycast on the deck normal to check if there's a card, if there isn't then create one.
    private bool CheckCard(){
        Ray ray;
        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.up);     //wallFront = Physics.Raycast(startPosition, orientation.forward, out frontWallHit, detectionLength, whatIsGround);

        Debug.DrawRay(transform.position,  Vector3.up, Color.blue);

        return Physics.Raycast(ray, out hit, 2f, whatIsCard);
    }

    //create an empty card on top of the deck, it is later initialized when clicked
    private void CreateEmptyCard(){
        if(cards.Count > 0){
            Vector3 instatiatePosition = new Vector3(transform.position.x, transform.position.y + drawCardOffsetY, transform.position.z);
            Quaternion simpleCardRotation = new Quaternion(0, 0, 180, 1);
            Instantiate(simpleCard, instatiatePosition, simpleCardRotation, transform);
            
            if(cards.Count < halfDeckThreshold){
                //change to half deck mesh
                halfDeck.SetActive(true);
                fullDeck.SetActive(false);
            }
        }
        else{
            //col.enabled = false;
            fullDeck.SetActive(false);
            halfDeck.SetActive(false);
            //meshRenderer.enabled = false;
        }
    }

    //Initialize the top card of the deck. Called from the card
    public void InitializeCard(CardObject cardObject){
        Card card = cards[cards.Count -1];
        cards.RemoveAt(cards.Count -1);
        cardObject.SetValueAndSuit(card.value, card.suitChar);
        cardObject.SetMaterial(card.material);

    }

    //function called at the end of the round, put all the cards that have been played on the bottom of the deck
    public void PutCardsAtBottom(){
        /*
            One way to do that is to search all the children of the deck (eexcept the last one) and add their card values to the deck.
        */

        //reset deck so that I can see all the deck if it had 0 cards
        //col.enabled = true;
        fullDeck.SetActive(true);
        halfDeck.SetActive(false);
        //meshRenderer.enabled = true;

        //Check if I have no card in the deck
        int childListLength;
        if(cards.Count == 0){
            //last card havee been played, remove it too
            childListLength = transform.childCount;
            Debug.Log("All Cards");
        }else{
            //childCount -1 becase the last card has not been played and so it has to stay on the top!
            childListLength = transform.childCount -1;
            Debug.Log("All Cards minus the top one");
        }

        //i = 2 because deck has the half deck and the full deck as first children.
        for(int i = numberOfChilds; i < childListLength; i++){
            
            Debug.Log(i);
            int bottomCardValue = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetValue();
            char bottomCardSuit = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetSuit();
            Material bottomCardMaterial = transform.GetChild(i).gameObject.GetComponent<CardObject>().GetMaterial();
            Card bottomCard = new Card(bottomCardSuit, bottomCardValue, bottomCardMaterial);

            //insert card at the bottom of the list
            cards.Insert(0,bottomCard);

            Destroy(transform.GetChild(i).gameObject);
        }
        

    }

    //called by the deckShuffleAnimation, set the deck mesh to active
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
