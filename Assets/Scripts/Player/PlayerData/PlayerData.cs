using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("PlayerSounds")]
    public AudioClip winSound;
    public AudioClip lostSound;
}
