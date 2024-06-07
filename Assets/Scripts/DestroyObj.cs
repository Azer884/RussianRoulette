using UnityEngine;

public class DestroyObj : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = true;
    }

    private void Awake() 
    {
        Destroy(gameObject, 5f);
    }
}
