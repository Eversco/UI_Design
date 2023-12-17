using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Initialize(CinemachineVirtualCamera aimVirtualCamera);
    public void Shoot();
    public void AimDownSights();
    public void ExitADS();
    public string GetWeaponName();
    public void Unequip(Transform parent);
    public void Equip(Transform parent);
    public float GetZoom();
}

