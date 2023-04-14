using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    public Color onHoverColor;
    public Color normalColor;

    public void OnPointerEnter(PointerEventData eventData){
        //Debug.Log("Hover on button");
        ChangeTextColor(onHoverColor);
    }

    public void OnPointerExit(PointerEventData eventData){
        //Debug.Log("Hover eexit button");
        ChangeTextColor(normalColor);
    }

    private void ChangeTextColor(Color color){
        buttonText.color = color;
    }

    //called from the button click event
    public void ChangeBackToNormal(){
        buttonText.color = normalColor;
    }

}
