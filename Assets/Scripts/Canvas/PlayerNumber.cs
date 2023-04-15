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
        Debug.Log("Start game");
        playerNumberScriptable.numberOfPlayer = currentValue; 
        LevelManager.Instance.LoadScene(sceneName);
        //SceneManager.LoadScene(sceneName);
        //GameManager.Instance.StartGame(currentValue);
    }

    public void IncrementValue(){
        if(currentValue >= maxPlayerNumber){
            Debug.Log("error too big a number");
            return;
        }
        currentValue ++;
        numberPlayerInputField.text = currentValue.ToString();
    }

     public void DecrementValue(){
        if(currentValue <= minPlayerNumber){
            Debug.Log("error too low a number");
            return;
        }
        currentValue --;
        numberPlayerInputField.text = currentValue.ToString();
    }
}
