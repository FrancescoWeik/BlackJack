using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowableObject : MonoBehaviour
{
    protected Rigidbody rb;
    protected AudioSource audioSource;

    //mouse coordinates
    protected Vector3 mOffset;
    protected float mZCoord;
    protected Vector3 startMousePosition; //mouse position before throwing


    Vector3 startPosition; //used to know where to throw the card
    Vector3 lastPosition; //last position before releasing the card
    public float forceMultiplier = 10f;
    public float yOffset = 0.2f; //y to apply during the throw

    protected bool isInteractable; 

    protected bool isDragging;

    protected virtual void FixedUpdate(){
        if(isDragging){

            startPosition = transform.position;
            startMousePosition = Camera.main.WorldToScreenPoint(startPosition);

            Vector3 newPosition = GetMouseWorldPos() + mOffset;
            rb.MovePosition(newPosition);
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        isInteractable = true;
    }

    //Remove the gravity and rotation, get object coordinates with respect to the mouse
    protected virtual void OnMouseDown(){
        if(isInteractable){
            isDragging = true;
            //freeze card rotation
            rb.freezeRotation = true;
            rb.useGravity = false;

            rb.detectCollisions = true;
            
            mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
            mOffset = transform.position - GetMouseWorldPos();

            startPosition = transform.position;
        }
    }

    //Called when releasing the throwable object
    protected virtual void OnMouseUp(){
        if(isInteractable){
            isDragging = false;
            rb.freezeRotation = false;
            rb.useGravity = true;

            Throw();
        }
    }

    protected virtual Vector3 GetMouseWorldPos(){

        //pixel coordinates
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    //Throw the object based on the y and x position of the mouse. The y corresponds to the z during the throw, while the y of the object during the throw is fixed.
    protected virtual void Throw(){

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
        rb.velocity = movementVector * forceMultiplier * 100 * Time.fixedDeltaTime;
        rb.AddTorque(new Vector3(0,10f,0), ForceMode.Impulse);

    }

    public virtual void SetVelocity(Vector3 velocity){
        rb.velocity = velocity;
    }

    //user cannot interact with the object anymore
    public virtual void RemoveInteraction(){
        isInteractable = false;
    }

    protected virtual void PlaySound(AudioClip audio){
        audioSource.PlayOneShot(audio);
    }

}
