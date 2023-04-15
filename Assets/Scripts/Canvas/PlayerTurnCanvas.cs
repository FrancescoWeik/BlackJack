using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnCanvas : MonoBehaviour
{

    //called at the start of the animation
    public void StartAnim(){
        //disable user clicks
        Cursor.lockState = CursorLockMode.Locked;

    }

    //called from the eend of the animation
    public void FinishAnim(){
        //enable user clicks
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(false);
    }
}
