using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBulletCollision : MonoBehaviour
{
    public RagdollActivator ragdollActivator;

    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.CompareTag("Bullet"))
        {
            ragdollActivator.Die();
        }
    }
}
