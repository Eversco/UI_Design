using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour { 
    [SerializeField] private Button serverButton; // No client. Server only
    [SerializeField] private Button hostButton; // Host = client + server
    [SerializeField] private Button clientButton; // No server. Client only

    private void Awake()
    {
        serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }

}
