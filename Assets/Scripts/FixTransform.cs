using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.SetLocalPositionAndRotation(Vector3.down, Quaternion.identity);
    }
}
