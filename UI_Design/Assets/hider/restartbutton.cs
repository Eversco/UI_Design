using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartbutton : MonoBehaviour
{
    //[SerializeField] GameObject hider;
    // Start is called before the first frame update
    public void restart()
    {
        SceneManager.LoadScene(0);
    }
}
