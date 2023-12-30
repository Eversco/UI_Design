using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    public GameObject hiderPrefab;
    public GameObject seekerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // For host
        {
            // Spawn a hider by default for the host or based on your game logic
            GameObject hider = Instantiate(hiderPrefab);
            hider.GetComponent<NetworkObject>().Spawn();
        }
        else // For clients
        {
            // Here you can implement logic to choose to spawn as a seeker
            RequestToSpawnSeekerServerRpc();
        }
    }

    [ServerRpc]
    void RequestToSpawnSeekerServerRpc(ServerRpcParams rpcParams = default)
    {
        // Server handles the request and spawns a seeker prefab for the client
        GameObject seeker = Instantiate(seekerPrefab);
        seeker.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
    }
}
