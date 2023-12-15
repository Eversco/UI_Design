using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;

public class ThirdPersonMorphController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private GameObject currentMorphObject;

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
            
            if (starterAssetsInputs.morph)
            {
                GameObject targetObject = rayCastHit.transform.gameObject;
                if (targetObject.GetComponent<MorphTarget>() != null)
                {
                    Debug.Log("Morphable");
                    if (currentMorphObject != null)
                    {
                        Destroy(currentMorphObject);
                    }
                    playerMeshRenderer.enabled = false;
                    currentMorphObject = Instantiate(targetObject);
                    currentMorphObject.transform.parent = transform;
                    if(currentMorphObject.TryGetComponent(out BoxCollider boxCollider))
                    {
                        boxCollider.isTrigger = true;
                    }
                    
                }
                else
                {
                    Debug.Log("Not morphable");
                }
                Debug.Log(rayCastHit.transform.gameObject.ToString() + " " + Vector3.Distance(rayCastHit.point, playerCameraRoot.position).ToString());
                starterAssetsInputs.morph = false;
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
        if(currentMorphObject != null)
        {
            currentMorphObject.transform.position = transform.position;
        }
    }
}
