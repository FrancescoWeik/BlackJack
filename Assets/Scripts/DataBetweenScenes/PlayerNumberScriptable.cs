using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//object used to pass the number of players between the menu scene and the game scene
[CreateAssetMenu(fileName = "playerNumberData", menuName = "Data/playerNumber")]
public class PlayerNumberScriptable : ScriptableObject
{
    public int numberOfPlayer;
}
