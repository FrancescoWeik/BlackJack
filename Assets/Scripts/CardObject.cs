using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour
{
    private Rigidbody rb;

    private int value;
    private char suitChar;
    private MeshRenderer meshRenderer;

    private bool alreadyInitialized;
    private Deck deck;

    //mouse coordinates
    private Vector3 mOffset;
    private float mZCoord;

    Vector3 startPosition; //used to know where to throw the card
    Vector3 lastPosition; //last position before releasing the card
    public float forceMultiplier = 10f;
    public float turnSpeed = 10f;
    public float yOffset = 0.2f;

    private bool isInteractable; 

    public LayerMask whatIsPlayer;
    public LayerMask whatIsDealer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log(meshRenderer);
        alreadyInitialized = false;
        isInteractable = true;
        deck = transform.parent.gameObject.GetComponent<Deck>(); //reference to deck needed to know how to initialize card
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetValue(){
        return value;
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
    public void OnMouseDown(){
        if(isInteractable){
            //freeze card rotation
            rb.freezeRotation = true;
            
            mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
            mOffset = transform.position - GetMouseWorldPos();

            startPosition = transform.position;

            if(checkAlreadyInitialized()){
                //do not reinitialize card, do nothing
                //Debug.Log("Already Initialized");
            }else{
                alreadyInitialized = true;
                deck.InitializeCard(this);
                //Debug.Log("initialized Card");
            }
        }
    }

    public void OnMouseDrag(){
        if(isInteractable){
            transform.position = GetMouseWorldPos() + mOffset;
        }

        //TODO Check if you are standing in a point for too long, if you are then reset the startPosition
    }

    public void OnMouseUp(){
        if(isInteractable){
            rb.freezeRotation = false;

            //Check if on a player or dealer, if it is then drop it on player,
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsPlayer)){
                //assign card to player if asking for a card
                if(hit.collider.gameObject.GetComponent<Player>().CheckAssignCard()){
                    hit.collider.gameObject.GetComponent<Player>().AssignCard(gameObject);
                }else{
                    Throw();
                }
            }
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsDealer)){
                //assign card to dealer if possible
                Debug.Log(hit.collider.gameObject);
                if(hit.collider.gameObject.GetComponent<DealerHand>().CheckAssignCard()){
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

    private Vector3 GetMouseWorldPos(){

        //pixel coordinates
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void Throw(){
        lastPosition = transform.position;
        Vector3 directionXY = (lastPosition - startPosition).normalized;
        Vector3 directionXZ = new Vector3(directionXY.x, yOffset, directionXY.y);
        Debug.DrawLine (startPosition, startPosition + directionXZ * 10, Color.red, Mathf.Infinity);

        //apply force in the direction
        rb.AddForce(directionXZ * forceMultiplier,ForceMode.Impulse);
        rb.AddTorque(new Vector3(0,10f,0), ForceMode.Impulse);
    }

    public void SetVelocity(Vector3 velocity){
        rb.velocity = velocity;
    }

    public void RemoveInteraction(){
        isInteractable = false;
    }


}
