using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour
{
    private int value;
    private char suitChar;
    private MeshRenderer meshRenderer;

    private bool alreadyInitialized;
    private Deck deck;

    //mouse coordinates
    private Vector3 mOffset;
    private float mZCoord;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log(meshRenderer);
        alreadyInitialized = false;
        deck = transform.parent.gameObject.GetComponent<Deck>(); //reference to deck needed to know how to initialize card
    }

    // Update is called once per frame
    void Update()
    {
        
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
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mOffset = transform.position - GetMouseWorldPos();

        if(checkAlreadyInitialized()){
            //do not reinitialize card, do nothing
            Debug.Log("Already Initialized");
        }else{
            alreadyInitialized = true;
            deck.InitializeCard(this);
            Debug.Log("initialized Card");
        }
    }

    public void OnMouseDrag(){
        transform.position = GetMouseWorldPos() + mOffset;
    }

    public void OnPointerUp(PointerEventData eventData){
        Debug.Log("mouseUp");
    }

    private Vector3 GetMouseWorldPos(){

        //pixel coordinates
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void Throw(){
        //if the card gets thrown then I need to create a new card at the top of the deck
    }


}
