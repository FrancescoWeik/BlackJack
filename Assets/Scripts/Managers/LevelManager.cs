using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Animator transition;
    public float transitionTime = 1f;
    
    [SerializeField] private GameObject loaderCanvas;
    
    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(string sceneName){
        Debug.Log("Loading scene");
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    //Fade image in and out
    public IEnumerator LoadSceneRoutine(string sceneName){
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
