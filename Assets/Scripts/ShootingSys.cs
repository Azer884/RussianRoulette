using Unity.Netcode;
using UnityEngine;

public class ShootingSys : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private GameObject bullet;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f;
    private int currentPos = 0;
    public Reload reloading;
    public Transform BulletContainer;
    public Transform cam;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && IsOwner)
        { 
            ShootServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc()
    {
        if (reloading.reloaded && reloading.canShoot)
        {
            if (reloading.bulletPos == currentPos)
            {

                bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<NetworkObject>().Spawn();
                
                if (bullet.TryGetComponent<Rigidbody>(out var bulletRigidbody))
                {
                    Vector3 direction = cam.forward;

                    bulletRigidbody.rotation = Quaternion.LookRotation(direction);

                    bulletRigidbody.velocity = direction * bulletSpeed;
                }
                Destroy(bullet, bulletLifetime);

                reloading.reloaded = false;
                reloading.animators[0].Play("Shooting");
                reloading.ammo.text = "0/6";
            }
            currentPos++ ;
            currentPos  %= 6;
        }

        if (!reloading.animators[0].GetCurrentAnimatorStateInfo(0).IsName("ReloadRevolver"))
        {
            reloading.animators[0].Play("Triggering");
            reloading.animators[2].Play("TriggeringArm");
        }
    
    }
}