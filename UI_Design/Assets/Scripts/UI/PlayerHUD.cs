using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] public TMP_Text currentBulletText;
    [SerializeField] public TMP_Text maxBulletText;

    [SerializeField] public TMP_Text reloader;

    [SerializeField] public Image hpBar;

    public void UpdateBullets(int currentBullet, int maxBullet)
    {
        currentBulletText.text = currentBullet.ToString();
        maxBulletText.text = maxBullet.ToString();

        //Debug.Log(currentBulletText.text);
    }

    public void UpdateHealth(int currentHealth)
    {
        hpBar.fillAmount = (float) currentHealth / 100f;

        //Debug.Log(currentBulletText.text);
    }

    public void Reloading()
    {
       reloader.text = "Reloading...";
    }

    public void NotReloading()
    {
        reloader.text = "";
    }
}
