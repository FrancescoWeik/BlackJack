using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerNumber : MonoBehaviour
{

    public InputField numberPlayerInputField;
    private int currentValue;
    public int maxPlayerNumber;
    public int minPlayerNumber;
    public PlayerNumberScriptable playerNumberScriptable;

    private void Start(){
        currentValue = 1;
        numberPlayerInputField.text = currentValue.ToString();
    }

    public void StartGame(string sceneName){
        playerNumberScriptable.numberOfPlayer = currentValue; 
        LevelManager.Instance.LoadScene(sceneName);
    }

    public void IncrementValue(){
        if(currentValue >= maxPlayerNumber){
            return;
        }
        currentValue ++;
        numberPlayerInputField.text = currentValue.ToString();
    }

     public void DecrementValue(){
        if(currentValue <= minPlayerNumber){
            return;
        }
        currentValue --;
        numberPlayerInputField.text = currentValue.ToString();
    }
}
