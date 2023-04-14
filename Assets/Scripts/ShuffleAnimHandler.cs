using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleAnimHandler : MonoBehaviour
{
    public Deck deck;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    //play the shuffle sound
    public void PlaySound(){
        audioSource.PlayOneShot(audioClip);
    }

    //called when deck finishes the shuffle animation
    public void FinishAnim(){
        audioSource.Stop();
        deck.FinishShuffleAnim();
    }
}
