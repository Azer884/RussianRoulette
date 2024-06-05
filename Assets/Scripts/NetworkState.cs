using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkState : NetworkBehaviour
{
    [HideInInspector]public NetworkVariable<bool> isDead = new NetworkVariable<bool>();
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        isDead.Value = false;       
    }
}