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
    [SerializeField] private Transform spawnGunPosition;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private List<Weapon> weapons;
    

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    public Weapon currentWeapon { get; private set; }
    private List<GameObject> instantiatedWeapons = new List<GameObject>();

    [SerializeField] private PlayerStat stat;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        
        //disable controller move rotation for now(rotation caused by player movement)
        thirdPersonController.SetRotateOnMove(false);
        foreach(Weapon weapon in weapons) 
        {
            Debug.Log(weapon.gameObject);
            GameObject instantiatedWeapon = Instantiate(weapon.gameObject, spawnGunPosition);
            instantiatedWeapon.GetComponent<Weapon>().InitializeGunStats(gameObject);
            instantiatedWeapons.Add(instantiatedWeapon);
            instantiatedWeapon.SetActive(false);
            instantiatedWeapon.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            instantiatedWeapon.transform.rotation *= Quaternion.Euler(0, -90, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mouseWorldPosition;
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

            if (starterAssetsInputs.shoot)
            {
                if(currentWeapon.CanShoot())
                {
                    currentWeapon.Shoot(mouseWorldPosition, pfBulletProjectile, debugRayDistance, aimColliderLayerMask);
                    stat.ReduceBullet();
                    //Debug.Log(spawnGunPosition.position);
                    //Works as a semi auto gun. Have to repress mb1 to shoot again
                }
            }
            else
            {
                currentWeapon.StopShoot();
            }

            if(starterAssetsInputs.reload)
            {
                starterAssetsInputs.reload = false;
                if (currentWeapon.CanReload())
                {
                    currentWeapon.Reload();
                    stat.Reload(currentWeapon.GetClipSize());
                }
                else
                {
                    Debug.Log("Current weapon is already reloading or clip full!");
                    if (currentWeapon.IsReloading()) stat.Reloaded();
                
                }
            }
            if (currentWeapon != null && !currentWeapon.IsReloading())
            {
                stat.NotReloaded();
            }
        }
        


        

        //will definitely refactor lol
        if(starterAssetsInputs.equipWeapon1)
        {
            EquipWeapon(instantiatedWeapons[0].GetComponent<Weapon>());
            starterAssetsInputs.equipWeapon1 = false;
        }
        if(starterAssetsInputs.equipWeapon2)
        {
            EquipWeapon(instantiatedWeapons[1].GetComponent<Weapon>());
            starterAssetsInputs.equipWeapon2 = false;
        }
        if(starterAssetsInputs.equipWeapon3)
        {
            EquipWeapon(instantiatedWeapons[2].GetComponent<Weapon>());
            starterAssetsInputs.equipWeapon3 = false;
        }
        if(starterAssetsInputs.equipWeapon4)
        {
            EquipWeapon(instantiatedWeapons[3].GetComponent<Weapon>());
            starterAssetsInputs.equipWeapon4 = false;
        }

        
        
    }
    public void EquipWeapon(Weapon weapon)
    {
        
        if(weapon == currentWeapon)
        {
            return;
        }
        //new weapon, need change
        //Debug.Log(weapon.GetWeaponName());
        if (currentWeapon != null)
        {
            currentWeapon.Unequip();
        }
        currentWeapon = weapon;
        currentWeapon.InitializeCamera(aimVirtualCamera, normalVirtualCamera);
        currentWeapon.Equip();

        stat.CheckGun(currentWeapon);
    }
}
