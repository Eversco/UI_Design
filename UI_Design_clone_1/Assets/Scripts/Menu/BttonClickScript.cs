using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickScript : MonoBehaviour
{

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

}
