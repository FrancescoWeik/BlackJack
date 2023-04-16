using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : ThrowableObject
{
    private int value;
    private char suitChar;
    private MeshRenderer meshRenderer;

    private bool alreadyInitialized;
    private Deck deck;

    public float turnSpeed = 10f;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsDealer;

    [SerializeField] private CardData cardData; //scriptable object containing some variables

    private bool assigned; //track whether or not the card has been assigned

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        meshRenderer = GetComponent<MeshRenderer>();

        //Debug.Log(meshRenderer);
        alreadyInitialized = false;
        deck = transform.parent.gameObject.GetComponent<Deck>(); //reference to deck needed to know how to initialize card
        assigned = false;
    }

    public int GetValue(){
        return value;
    }

    public char GetSuit(){
        return suitChar;
    }

    public Material GetMaterial(){
        return this.meshRenderer.material;
    }

    //set the value and suit of the card
    public void SetValueAndSuit(int value, char suitChar){
        this.value = value;
        this.suitChar = suitChar;
    }

    //assign the material to the card
    public void SetMaterial(Material material){
        //Debug.Log(material);
        this.meshRenderer.material = material;
        alreadyInitialized = true;
    }

    //this function checks if you already assigned the material, if you did then you don-t have to pull a new card from the deck.
    public bool checkAlreadyInitialized(){
        if(alreadyInitialized){
            return true;
        }else{
            return false;
        }
    }

    //When a card is clicked check if it has been initialized, if not then initialize it
    protected override void OnMouseDown(){
        base.OnMouseDown();

        if(checkAlreadyInitialized()){
                //do not reinitialize card, do nothing
        }else{
            alreadyInitialized = true;
            deck.InitializeCard(this);
        }
    }

    //Called when releasing the card, check if hitting a player or the dealer field, if you are then assign the card, else throw the card
    protected override void OnMouseUp(){
        if(isInteractable){
            isDragging = false;
            rb.freezeRotation = false;
            rb.useGravity = true;

            //Check if on a player or dealer, if it is then drop it on player,
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsPlayer)){
                //assign card to player if asking for a card
                if(hit.collider.gameObject.GetComponent<Player>().CheckAssignCard()){
                    PlaySound(cardData.flipCardSound);
                    hit.collider.gameObject.GetComponent<Player>().AssignCard(gameObject);
                }else{
                    Throw();
                }
            }
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsDealer)){
                //assign card to dealer if possible
                if(hit.collider.gameObject.GetComponent<DealerHand>().CheckAssignCard()){
                    PlaySound(cardData.flipCardSound);
                    hit.collider.gameObject.GetComponent<DealerHand>().AssignCard(gameObject);
                }else{
                    Throw();
                }
            }
            else{
                //if card is not on a player or on the dealer then throw it
                Throw();
            }
        }
    }

    protected override Vector3 GetMouseWorldPos(){
        return base.GetMouseWorldPos();
    }

    //Throw the card based on the y and x position of the mouse. The y corresponds to the z during the throw, while the y of the card during the throw is fixed.
    protected override void Throw(){

        //play sound
        PlaySound(cardData.throwCardSound);

        base.Throw();

    }

    public override void SetVelocity(Vector3 velocity){
        base.SetVelocity(velocity);
    }

    //user cannot interact with the card anymore (it's either on a player hand or on the dealer field)
    public override void RemoveInteraction(){
        base.RemoveInteraction();
    }

    protected override void PlaySound(AudioClip audio){
        base.PlaySound(audio);
    }

    public void SetAssigned(){
        assigned = true;

        //call function after some seconds
        Invoke("RemoveRigidBody", 0.5f);
    }

    //make the card rigidbody kinematic so that other cards cannot push it
    private void RemoveRigidBody(){
        rb.isKinematic = true;
    }

    public bool GetAssigned(){
        return assigned;
    }

}
