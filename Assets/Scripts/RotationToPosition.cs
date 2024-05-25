using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RotationToPosition : NetworkBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            UpdateTargetPosition();
            UpdateTargetPositionServerRpc(transform.localPosition);
        }
    }

    private void UpdateTargetPosition()
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            cam.localRotation.x * 15
        );
    }


    [ServerRpc]
    private void UpdateTargetPositionServerRpc(Vector3 newPosition)
    {
        UpdateTargetPositionClientRpc(newPosition);
    }

    [ClientRpc]
    private void UpdateTargetPositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner)
        {
            transform.localPosition = newPosition;
        }
    }

}
