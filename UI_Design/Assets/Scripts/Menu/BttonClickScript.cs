using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickScript : MonoBehaviour
{
    [SerializeField] GameObject Menu;

    public void StartGameAsHost()
    {

        SceneManager.LoadScene(1);

    }
    public void StartGameAClient()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void backtomenu()
    {
        SceneManager.LoadScene(0);
    }

    public void resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Menu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
