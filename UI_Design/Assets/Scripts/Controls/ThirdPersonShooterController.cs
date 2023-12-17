using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEditor.ShaderGraph.Internal;

public class ThirdPersonShooterController: MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera normalVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private List<Weapon> weapons;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Weapon currentWeapon;


    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        
        //disable controller move rotation for now(rotation caused by player movement)
        thirdPersonController.SetRotateOnMove(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        float debugRayDistance = 100f;
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, debugRayDistance, aimColliderLayerMask))
        {
            debugTransform.position = rayCastHit.point;
            mouseWorldPosition = rayCastHit.point;
        }
        else
        {
            debugTransform.position = Camera.main.transform.position + Camera.main.transform.forward * debugRayDistance;
            mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * debugRayDistance;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        //rotate the character with a bit of lerp
        //transform.forward = aimDirection;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        if(currentWeapon != null)
        {
            if (starterAssetsInputs.aim)
            {
                currentWeapon.AimDownSights();
                thirdPersonController.SetSensitivity(aimSensitivity);
            }
            else
            {
                currentWeapon.ExitADS();
                thirdPersonController.SetSensitivity(normalSensitivity);
            }
        }
        


        if(starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                
            //Works as a semi auto gun. Have to repress mb1 to shoot again
            starterAssetsInputs.shoot = false;
        }

        //will definitely refactor lol
        if(starterAssetsInputs.equipWeapon1)
        {
            EquipWeapon(weapons[0]);
            starterAssetsInputs.equipWeapon1 = false;
        }
        if(starterAssetsInputs.equipWeapon2)
        {
            EquipWeapon(weapons[1]);
            starterAssetsInputs.equipWeapon2 = false;
        }
        if(starterAssetsInputs.equipWeapon3)
        {
            EquipWeapon(weapons[2]);
            starterAssetsInputs.equipWeapon3 = false;
        }

        
        
    }
    public void EquipWeapon(Weapon weapon)
    {
        
        if(weapon == currentWeapon)
        {
            return;
        }
        //new weapon, need change
        Debug.Log(weapon.GetWeaponName());
        if (currentWeapon != null)
        {
            currentWeapon.Unequip(spawnBulletPosition);
        }
        currentWeapon = weapon;
        currentWeapon.Initialize(aimVirtualCamera, normalVirtualCamera);
        currentWeapon.Equip(spawnBulletPosition);

    }
}
