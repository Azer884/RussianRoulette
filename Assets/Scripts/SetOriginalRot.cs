using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOriginalRot : MonoBehaviour
{
    public Transform originalTransform;
    // Start is called before the first frame update
    void Start()
    {
        originalTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = originalTransform.rotation;
    }
}
