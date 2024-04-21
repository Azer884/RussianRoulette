using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjRotaition : MonoBehaviour
{
    public Transform followedObj;
    void Update()
    {
        transform.rotation = followedObj.rotation;
    }
}
