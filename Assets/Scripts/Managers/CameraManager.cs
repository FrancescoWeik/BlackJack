using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{   
    [Header("CameraGOParent")]
    public GameObject personPerspectiveCameraGO;
    public GameObject standardCameraGO;

    [Header("Camera")]
    public Camera standardCamera;
    public Camera personPerspectiveCamera;

    private bool isPerspective;

    [Header("UI")]
    public GameObject worldButtonCanvas; //need to reference it because it has to change camera to keep working in both perspectives

    private void Start(){
        isPerspective = false;
    }

    void Update()
    {  
        if(Input.GetKeyDown(KeyCode.C)){
            if(isPerspective){
                standardCameraGO.SetActive(true);
                personPerspectiveCameraGO.SetActive(false);
                isPerspective = false;
                worldButtonCanvas.GetComponent<Canvas>().worldCamera = standardCamera;
            }else{
                personPerspectiveCameraGO.SetActive(true);
                standardCameraGO.SetActive(false);
                isPerspective = true;
                worldButtonCanvas.GetComponent<Canvas>().worldCamera = personPerspectiveCamera;
            }
        }
    }
}
