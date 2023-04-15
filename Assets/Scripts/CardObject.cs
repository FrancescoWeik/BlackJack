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

    private AudioSource audioSource;

    [SerializeField] private CardData cardData; //scriptable object containing some variables

    private bool isDragging;

    private Vector3 startMousePosition;

    public void FixedUpdate(){
        if(isDragging){

            startPosition = transform.position;
            startMousePosition = Camera.main.WorldToScreenPoint(startPosition);

            Vector3 newPosition = GetMouseWorldPos() + mOffset;
            rb.MovePosition(newPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        //Debug.Log(meshRenderer);
        alreadyInitialized = false;
        isInteractable = true;
        deck = transform.parent.gameObject.GetComponent<Deck>(); //reference to deck needed to know how to initialize card
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
    public void OnMouseDown(){
        if(isInteractable){
            isDragging = true;
            //freeze card rotation
            rb.freezeRotation = true;
            rb.useGravity = false;

            rb.detectCollisions = true;
            
            mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
            mOffset = transform.position - GetMouseWorldPos();

            startPosition = transform.position;

            if(checkAlreadyInitialized()){
                //do not reinitialize card, do nothing
            }else{
                alreadyInitialized = true;
                deck.InitializeCard(this);
            }
        }
    }

    //Called when releasing the card, check if hitting a player or the dealer field, if you are then assign the card, else throw the card
    public void OnMouseUp(){
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

    private Vector3 GetMouseWorldPos(){

        //pixel coordinates
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    //Throw the card based on the y and x position of the mouse. The y corresponds to the z during the throw, while the y of the card during the throw is fixed.
    private void Throw(){
        //play sound
        PlaySound(cardData.throwCardSound);

        //get the last mouse position
        Vector3 lastMousePosition = Camera.main.WorldToScreenPoint(transform.position);

        //get the difference between the previous mouse position and the current one
        float mouseXMovement = lastMousePosition.x - startMousePosition.x;
        float mouseYMovement = lastMousePosition.y - startMousePosition.y;

        Vector3 newPositionZ;
        Vector3 newPositionX;

        Vector3 newPositionZNoY;
        Vector3 newPositionXNoY;

        //if the mouse movement is 0 then do not throw the card in the x direction
        if(mouseXMovement == 0){
            newPositionXNoY = new Vector3(0,0,0);
        }else{
            //calculate the x direction with respect to the camera rotation
            newPositionX = transform.position + (Camera.main.transform.right * mouseXMovement);
            newPositionXNoY = new Vector3(newPositionX.x, yOffset, newPositionX.z);
        }
        //if the mouse movement is 0 or less then do not throw the card in the z direction (we are converting the y mouse position to the z direction)
        if(mouseYMovement <=0){
            newPositionZNoY = new Vector3(0,0,0);
        }else{
            //calculate the z direction with respect to the camera rotation
            newPositionZ = transform.position + (Camera.main.transform.forward * mouseYMovement);
            newPositionZNoY = new Vector3(newPositionZ.x, yOffset, newPositionZ.z);
        }

        //find the result vector of the previous 2
        Vector3 movementVector = (newPositionZNoY + newPositionXNoY).normalized;

        //apply velocity in the direction of the vector.
        rb.velocity = movementVector * forceMultiplier * 100 * Time.deltaTime;
        rb.AddTorque(new Vector3(0,10f,0), ForceMode.Impulse);

    }

    public void SetVelocity(Vector3 velocity){
        rb.velocity = velocity;
    }

    //user cannot interact with the card anymore (it's either on a player hand or on the dealer field)
    public void RemoveInteraction(){
        isInteractable = false;
    }

    private void PlaySound(AudioClip audio){
        audioSource.PlayOneShot(audio);
    }

}
