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
    public ShootingSys shoot;
    public GameObject hand;
    private Rigidbody[] ragdollRigids;

    //private NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);

    void Start()
    {
        GetRagdollBits();
        RagdollModelOff();
    }
    private void OnEnable() 
    {
        GetComponent<NetworkState>().isDead.OnValueChanged += StateChange;
    }
    private void OnDisable() 
    {
        GetComponent<NetworkState>().isDead.OnValueChanged -= StateChange;
    }

    private void StateChange(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            RagdollModeOn();
        }
    }

    void Update()
    {
        if (GetComponent<NetworkState>().isDead.Value)
        {
            camScrpt.transform.LookAt(ragdollRigids[5].transform);
        }
    }

    public void RagdollModeOn()
    {
        animator.enabled = false;
        camScrpt.enabled = false;
        mvmnt.enabled = false;
        shoot.enabled = false;
        hand.SetActive(false);
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
        shoot.enabled = true;
        hand.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void GetRagdollBits()
    {
        ragdollRigids = theRig.GetComponentsInChildren<Rigidbody>();
    }

    /*public void Die()
    {
        if (IsOwner)
        {
            isDead.Value = true;
            UpdateRagdollStateOnServerRpc(true);
        }
    }

    [ServerRpc]
    private void UpdateRagdollStateOnServerRpc(bool deadState)
    {
        isDead.Value = deadState;
        UpdateRagdollStateOnClientRpc(deadState);
    }

    [ClientRpc]
    private void UpdateRagdollStateOnClientRpc(bool deadState)
    {
        isDead.Value = deadState;
        if (deadState)
        {
            RagdollModeOn();
        }
        else
        {
            RagdollModelOff();
        }
    }*/
}
