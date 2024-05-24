using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DropWeight : MonoBehaviour
{
    public PlayerMovementAdvanced mvmnt;
    private MultiAimConstraint multiAimConstraint;
    private float weight;
    // Start is called before the first frame update
    void Start()
    {
        multiAimConstraint = GetComponent<MultiAimConstraint>();
        weight = multiAimConstraint.weight;
    }

    // Update is called once per frame
    void Update()
    {
        if (mvmnt.state == PlayerMovementAdvanced.MovementState.crouching)
        {
            multiAimConstraint.weight = 0f;
        }
        else{
            multiAimConstraint.weight = weight;
        }
    }
}
