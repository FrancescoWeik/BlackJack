using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoundCanvas : MonoBehaviour
{
    //called from the animation
    public void FinishAnim(){
        
        GameManager.Instance.StartNextRound();

        gameObject.SetActive(false);
    }
}
