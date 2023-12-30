using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public GameObject hiderPrefab;
    public GameObject seekerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Make sure we are on the server when spawning objects
        {
            GameObject chosenPrefab = hiderPrefab;
            GameObject spawnedObject = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
            spawnedObject.GetComponent<NetworkObject>().Spawn(); // Spawn the object over the network

            Debug.Log(chosenPrefab);

            // Optionally set the parent of the spawned object
            spawnedObject.transform.SetParent(transform, true);
        }
        else if (!IsOwner)
        {
            GameObject chosenPrefab = seekerPrefab;
            GameObject spawnedObject = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
            spawnedObject.GetComponent<NetworkObject>().Spawn(); // Spawn the object over the network

            Debug.Log("SEEEEEE");

            // Optionally set the parent of the spawned object
            spawnedObject.transform.SetParent(transform, true);
        }
    }

}
