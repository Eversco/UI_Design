using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.Linq;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine.UIElements;
using UnityEditor;

public class ThirdPersonMorphController : NetworkBehaviour
{

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;


    public NetworkPrefabsList morphablePrefabs;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private GameObject currentMorphObject;

    private NetworkVariable<bool> isMorphed = new NetworkVariable<bool>(false); 

    private void Awake()
    {
        /*
        if (aimVirtualCamera == null)
        {
            aimVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
        */
    }

    public override void OnNetworkSpawn()
    {
        if (isMorphed.Value) {
            playerMeshRenderer.enabled = false;
        }
        aimVirtualCamera.gameObject.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

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
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, debugRayDistance, aimColliderLayerMask))
        {
            debugTransform.position = rayCastHit.point;
            mouseWorldPosition = rayCastHit.point;

            if (starterAssetsInputs.morph)
            {
                starterAssetsInputs.morph = false;
                GameObject targetObject = rayCastHit.transform.gameObject;
                if (targetObject.GetComponent<MorphTarget>() != null)
                {
                    if (isMorphed.Value)
                    {
                        currentMorphObject.GetComponent<NetworkObject>().Despawn();
                        isMorphed.Value = false;
                        playerMeshRenderer.enabled = true;
                    }
                    foreach (NetworkPrefab prefab in morphablePrefabs.PrefabList)
                    {
                        if (targetObject.name.Contains(prefab.Prefab.name)) {
                            MorphIntoTargetServerRpc(prefab.Prefab.name);
                            isMorphed.Value = true;
                            break;
                        }
                    }
                    
                    // This needs to be a rpc
                    


                }
                else
                {
                    Debug.Log("Not morphable");
                }
                Debug.Log(rayCastHit.transform.gameObject.ToString() + " " + Vector3.Distance(rayCastHit.point, playerCameraRoot.position).ToString());

            }
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
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 5f);

        //debug
        if (currentMorphObject != null)
        {
            currentMorphObject.transform.position = transform.position;
        }
    }
    [ServerRpc]
    void MorphIntoTargetServerRpc(string prefabName, ServerRpcParams rpcParams = default)
    {

        playerMeshRenderer.enabled = false;

        foreach (NetworkPrefab prefab in morphablePrefabs.PrefabList)
        {
            if (prefab.Prefab.name.Contains(prefabName)) {
                currentMorphObject = Instantiate(prefab.Prefab);
            }
        }
        
        //Necessary modifications to the object for things to properly work
        
        
        if (currentMorphObject.TryGetComponent(out Collider collider))
        {
            collider.isTrigger = true;
        }
        currentMorphObject.GetComponent<NetworkObject>().Spawn();
        currentMorphObject.transform.parent = transform;
        MorphIntoTargetClientRpc();
    }

    [ClientRpc]
    void MorphIntoTargetClientRpc()  {
        playerMeshRenderer.enabled = false;
    }
}
