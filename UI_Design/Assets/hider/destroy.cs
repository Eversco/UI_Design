using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class destroy : MonoBehaviour
{
    [SerializeField] public GameObject winimage;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        winimage.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
