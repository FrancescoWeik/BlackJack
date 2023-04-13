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

    private MeshRenderer meshRenderer;
    private Collider col;

    private void Start(){
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();

        InitializeDeck();
    }

    private void Update(){
        if(!CheckCard()){
            //Debug.Log("Card missing");
            CreateEmptyCard();
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

    //shoot a raycast on the deck normal to check if there's a card, if there isn't then create one.
    private bool CheckCard(){
        Ray ray;
        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.up);     //wallFront = Physics.Raycast(startPosition, orientation.forward, out frontWallHit, detectionLength, whatIsGround);

        Debug.DrawRay(transform.position,  Vector3.up, Color.blue);

        return Physics.Raycast(ray, out hit, 2f, whatIsCard);
    }

    private void CreateEmptyCard(){
        if(cards.Count > 0){
            //Debug.Log(transform.position.y + drawCardOffsetY);
            Vector3 instatiatePosition = new Vector3(transform.position.x, transform.position.y + drawCardOffsetY, transform.position.z);
            Quaternion simpleCardRotation = new Quaternion(0, 0, 180, 1);
            Instantiate(simpleCard, instatiatePosition, simpleCardRotation, transform);
        }else{
            //TODO remove deck if finished
            col.enabled = false;
            meshRenderer.enabled = false;
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

        //childCount -1 becase the last card has not been played and so it has to stay on the top!
        for(int i = 0; i < transform.childCount -1; i++){
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

    
    void printDeck(){
        for(int i=0; i<cards.Count; i++){
            cards[i].Print();
        }
        Debug.Log(cards.Count);
    }   

}
