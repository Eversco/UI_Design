using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public Transform gunModel;
    public Transform vfxHit;
    public Transform bullet;
    public Animation equipAnim;
    public Animation fireAnim;
    public Animation reloadAnim;
    public string weaponName;
    public float damage;
    public float firerate;
    public float zoomMultiplier;
    public int clipSize;
    public float reloadTime;
    [Tooltip("How many bullet instances fires at once, useful for shotguns")]
    public int pelletCount;
    [Tooltip("Random spread for each bullet fired in degrees")]
    public float inaccuracy;
    [Tooltip("damage * this = damage the shooter suffers upon shooting the wrong object")]
    public float penaltyMultiplier;
    public FireType fireType;


}

public enum FireType
{
    Semi,
    Burst,
    FullAuto
}