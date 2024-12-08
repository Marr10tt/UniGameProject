using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private bool uiEnabled;

    void Start(){
        uiEnabled = false;
        canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(uiEnabled == false){
                Pause();
            }
            else{

                Resume();
            }
        }
    }

    public void Pause(){
        canvas.enabled = true;
        uiEnabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("pausing");
    }
    public void Resume(){
        canvas.enabled = false;
        uiEnabled = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("resuming");
    }
    public void ExitGame(){
        Debug.Log("Exiting");
        Application.Quit();
    }
}
