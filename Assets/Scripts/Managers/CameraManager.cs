using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject personPerspectiveCamera;
    public GameObject standardCamera;

    private bool isPerspective;

    private void Start(){
        isPerspective = false;
    }

    void Update()
    {  
        if(Input.GetKeyDown(KeyCode.C)){
            if(isPerspective){
                standardCamera.SetActive(true);
                personPerspectiveCamera.SetActive(false);
                isPerspective = false;
            }else{
                personPerspectiveCamera.SetActive(true);
                standardCamera.SetActive(false);
                isPerspective = true;
            }
        }
    }
}
