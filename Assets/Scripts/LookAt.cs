using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetRotation;
    void Update()
    {
        Vector3 lookDirection = target.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(offsetRotation);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
