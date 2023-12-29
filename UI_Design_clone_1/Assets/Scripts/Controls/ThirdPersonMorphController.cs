using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine.UIElements;

public class ThirdPersonMorphController : NetworkBehaviour
{
    #region Serialized Fields
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    [Header("Transforms")]
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform playerCameraRoot;

    [Header("Renderer")]
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
    #endregion

    #region Private Fields
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private GameObject currentMorphObject;
    private Camera mainCamera;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CacheComponents();
    }

    public override void OnNetworkSpawn()
    {
        aimVirtualCamera.gameObject.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

    void Start()
    {
        SetupController();
    }

    void Update()
    {
        HandleAiming();
        HandleMorphing();
    }
    #endregion

    #region Custom Methods
    private void CacheComponents()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        mainCamera = Camera.main;
    }

    private void SetupController()
    {
        thirdPersonController.SetRotateOnMove(false);
    }

    private void HandleAiming()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            debugTransform.position = raycastHit.point;
        }
        else
        {
            debugTransform.position = mainCamera.transform.position + mainCamera.transform.forward * 100f;
            mouseWorldPosition = mainCamera.transform.position + mainCamera.transform.forward * 100f;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        // Rotate the character with a bit of lerp
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 5f);
    }


    private void HandleMorphing()
    {
        if (!starterAssetsInputs.morph) return;

        starterAssetsInputs.morph = false;
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, aimColliderLayerMask))
        {
            GameObject targetObject = raycastHit.transform.gameObject;

            if (targetObject.GetComponent<MorphTarget>() != null)
            {
                MorphObject(targetObject);
            }
            else
            {
                Debug.Log("Not morphable");
            }
        }

        
    }

    private void MorphObject(GameObject targetObject)
    {
        Debug.Log("Morphable");
        if (currentMorphObject != null)
        {
            Destroy(currentMorphObject);
        }

        // This needs to be an RPC
        MorphIntoTargetServerRpc();
        currentMorphObject = Instantiate(targetObject);

        // Netcode spawning
        if (currentMorphObject.GetComponent<NetworkObject>() != null)
        {
            currentMorphObject.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        }
        else
        {
            Debug.LogWarning("Target object does not have NetworkObject component!");
        }

        // Necessary modifications to the object for things to properly work
        ModifyMorphObject(currentMorphObject);
    }

    private void ModifyMorphObject(GameObject morphObject)
    {
        if (morphObject.GetComponent<HealthSystem>() != null)
        {
            Destroy(morphObject.GetComponent<HealthSystem>());
        }
        if (morphObject.GetComponent<MorphTarget>() != null)
        {
            Destroy(morphObject.GetComponent<MorphTarget>());
        }

        morphObject.AddComponent<PlayerMorphed>();
        morphObject.AddComponent<DamageParent>();
        morphObject.transform.parent = transform;
        if (morphObject.TryGetComponent(out Collider collider))
        {
            collider.isTrigger = true;
        }

        morphObject.transform.position = transform.position;
    }
    #endregion

    [ServerRpc]
    void MorphIntoTargetServerRpc(ServerRpcParams rpcParams = default)
    {
        playerMeshRenderer.enabled = false;
    }

    [ClientRpc]
    void MorphIntoTargetClientRpc(ClientRpcParams rpcParams = default)
    {
        playerMeshRenderer.enabled = false;
    }
}
