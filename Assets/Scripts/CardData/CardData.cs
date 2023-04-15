using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCardData", menuName = "Data/CardData")]
public class CardData : ScriptableObject
{
    [Header("CardSound")]
    public AudioClip flipCardSound;
    public AudioClip throwCardSound;
}
