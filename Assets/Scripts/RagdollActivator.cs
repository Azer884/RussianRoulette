using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    public GameObject theRig;
    public Animator animator;
    public FPSCam camScrpt;
    public PlayerMovementAdvanced mvmnt;
    public GameObject hand;
    private Rigidbody[] ragdollRigids;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        GetRagdollBits();
        RagdollModelOff();
    }
    void Update() {
        if (isDead)
        {
            camScrpt.transform.LookAt(ragdollRigids[5].transform);
        }    
    }

    public void RagdollModeOn()
    {
        animator.enabled = false;
        camScrpt.enabled = false;
        mvmnt.enabled = false;
        hand.SetActive(false);
        isDead = true;
        mvmnt.ChangeLayerRecursively(theRig, 9);
        

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
        camScrpt.enabled = true;
        mvmnt.enabled = true;
        hand.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void GetRagdollBits()
    {
        ragdollRigids = theRig.GetComponentsInChildren<Rigidbody>();
    }
}
