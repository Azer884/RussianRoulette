using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    public GameObject theRig;
    public Animator animator;
    private Rigidbody[] ragdollRigids;
    // Start is called before the first frame update
    void Start()
    {
        GetRagdollBits();
        RagdollModelOff();
    }

    public void RagdollModeOn()
    {
        animator.enabled = false;

        foreach (Rigidbody rigid in ragdollRigids)
        {
            rigid.isKinematic = false;
        }

        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void RagdollModelOff()
    {

        foreach (Rigidbody rigid in ragdollRigids)
        {
            rigid.isKinematic = true;
        }

        animator.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void GetRagdollBits()
    {
        ragdollRigids = theRig.GetComponentsInChildren<Rigidbody>();
    }
}
