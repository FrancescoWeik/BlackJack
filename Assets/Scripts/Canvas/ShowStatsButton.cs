using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStatsButton : MonoBehaviour
{
    public Text buttonText;
    private bool clicked;

    private void Start(){
        clicked = false;
    }
    
    public void ButtonClicked(){
        clicked = !clicked;

        if(clicked){
            PlayersManager.Instance.ShowPlayerStats();
            buttonText.text = "X";
        }else{
            PlayersManager.Instance.RemovePlayerStats();
            buttonText.text = "";
        }

    }
}
