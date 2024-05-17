using UnityEngine;
using Unity.Netcode;

public class CheckOwnership : NetworkBehaviour
{
    [SerializeField]private GameObject startButton;
    [SerializeField]private GameObject waitForOwner;
    void Update()
    {
        if (!IsHost)
        {
            startButton.SetActive(false);
            waitForOwner.SetActive(true);
        }
            
    }
}
