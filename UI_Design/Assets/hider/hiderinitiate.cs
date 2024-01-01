using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hiderinitiate : MonoBehaviour
{
    [SerializeField] GameObject canbehiderprops;
    [SerializeField]  GameObject winimage;
    // Start is called before the first frame update
    public void makehider()
    {
        Transform parenttransform = canbehiderprops.transform;
        int randomindex = Random.Range(0, parenttransform.childCount);
        Transform randomtransform = parenttransform.GetChild(randomindex);
        while (true)
        {
            if (randomtransform.name.Contains("Props_Roof") || randomtransform.name.Contains("Windmill") || randomtransform.name.Contains("Rock") || randomtransform.name.Contains("Bench") || randomtransform.name.Contains("Tree") || randomtransform.name.Contains("Bush"))
            {
                randomindex = Random.Range(0, parenttransform.childCount);
                randomtransform = parenttransform.GetChild(randomindex);
                continue;
            }
            else
            {
                break;
            }
        }

        /*Renderer targetrenderer=randomtransform.GetComponent<Renderer>();
        Renderer currentrenderer=GetComponent<Renderer>();
        currentrenderer.material = targetrenderer.material;

        Collider targetcollider=randomtransform.GetComponent<Collider>();
        Collider currentcollider=GetComponent<Collider>();
        Destroy(currentcollider);
        Collider newcollider = gameObject.AddComponent(targetcollider.GetType()) as Collider;
        transform.localPosition =new Vector3(130, 8, -1);
        */
        Transform newobject = Instantiate(randomtransform);
        //newobject.localPosition = new Vector3(130, 0, -1);
        //newobject.AddComponent<Rigidbody>();
        newobject.AddComponent<MeshCollider>();
        newobject.tag = "hider";
        destroy script = newobject.AddComponent<destroy>();
        script.winimage = winimage;
        //Debug.Log(newobject.name);

        if (newobject.name.Contains("Vehicle"))
        {
            int r = Random.Range(1, 4);
            int x = 0, z = 0;
            switch (r)
            {
                case 1:
                    int r1 = Random.Range(1, 5);
                    if (r1 == 1) { x = 118; } else if (r1 == 2) { x = 123; } else if (r1 == 3) { x = 237; } else if (r1 == 4) { x = 243; }
                    z = Random.Range(5, 219); break;
                case 2:
                    int r2 = Random.Range(1, 9);
                    if (r2 == 1) { z = -3; }
                    else if (r2 == 2) { z = 3; }
                    else if (r2 == 3) { z = 57; }
                    else if (r2 == 4) { z = 63; }
                    else if (r2 == 5) { z = 116; } else if (r2 == 6) { z = 123; } else if (r2 == 7) { z = 156; } else if (r2 == 8) { z = 163; }
                    x = Random.Range(120, 231); break;
                case 3:
                    int r3 = Random.Range(1, 3);
                    if (r3 == 1)
                    { x = 177; }
                    else
                    { x = 183; }
                    z = Random.Range(5, 105); break;

            }
            newobject.localPosition = new Vector3(x, 0, z);
        }


        /*else if (newobject.name.Contains("Windmill") || newobject.name.Contains("Rock") || newobject.name.Contains("Bench") || newobject.name.Contains("Tree") || newobject.name.Contains("Bush"))
        {
            Prop script = newobject.AddComponent<Prop>();
            script.currentHP = 20;
            script.propData = propData;
            Rigidbody rb =newobject.GetComponent<Rigidbody>();
            rb.drag = 100;

            int r= Random.Range(1,7);
            int x=0, z=0;
            switch (r)
            {
                case 1: x =Random.Range(251,269);z=Random.Range(-29,228); break;
                case 2: x = Random.Range(135, 229); z = Random.Range(130, 148); break;
                case 3: x = Random.Range(133, 228); z = Random.Range(170, 173); break;
                case 4: x = Random.Range(225, 228); z = Random.Range(12, 48); break;
                case 5: x = Random.Range(191, 199); z = Random.Range(12, 48); break;
                case 6: x = Random.Range(132, 169); z = Random.Range(99, 107); break;
            }
            newobject.localPosition = new Vector3(x, 0, z);
        }*/


        else if (newobject.name.Contains("Hydrant") || newobject.name.Contains("Dustbin") || newobject.name.Contains("chair") || newobject.name.Contains("Stop")
            || newobject.name.Contains("BillBoard") || newobject.name.Contains("Light") || newobject.name.Contains("Signal"))
        {

            int r = Random.Range(1, 4);
            int x = 0, z = 0;
            switch (r)
            {
                case 1:
                    int r1 = Random.Range(1, 5);
                    if (r1 == 1) { x = 114; } else if (r1 == 2) { x = 126; } else if (r1 == 3) { x = 234; } else if (r1 == 4) { x = 246; }
                    z = Random.Range(5, 219); break;
                case 2:
                    int r2 = Random.Range(1, 9);
                    if (r2 == 1) { z = -7; }
                    else if (r2 == 2) { z = 7; }
                    else if (r2 == 3) { z = 53; }
                    else if (r2 == 4) { z = 67; }
                    else if (r2 == 5) { z = 112; } else if (r2 == 6) { z = 127; } else if (r2 == 7) { z = 152; } else if (r2 == 8) { z = 167; }
                    x = Random.Range(120, 231); break;
                case 3:
                    int r3 = Random.Range(1, 3);
                    if (r3 == 1)
                    { x = 173; }
                    else
                    { x = 187; }
                    z = Random.Range(5, 105); break;

            }
            newobject.localPosition = new Vector3(x, 0, z);
        }

        else if (newobject.name.Contains("cone") || newobject.name.Contains("Fence"))
        {
            int x = 0, z = 0;
            x = Random.Range(236, 248); z = Random.Range(130, 153);
            newobject.localPosition = new Vector3(x, 0, z);
        }

        else
        {
            int x = 0, z = 0;
            x = Random.Range(120, 268); z = Random.Range(-29, 227);
            newobject.localPosition = new Vector3(x, 0, z);
        }
        //newobject.localPosition = new Vector3(130, 0, -1);
    }
    void Start()
    {
        makehider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
