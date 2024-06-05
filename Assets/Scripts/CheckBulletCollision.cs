using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckBulletCollision : NetworkBehaviour
{
    public RagdollActivator ragdollActivator;


    private void OnCollisionEnter(Collision other) 
    {
        if(!IsServer) return;
        if (other.transform.CompareTag("Bullet"))
        {
            //ragdollActivator.Die();
            ragdollActivator.transform.GetComponent<NetworkState>().isDead.Value = true;
        }
    }
}
