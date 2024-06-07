using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShootingSys : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private GameObject bullet;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f;
    public Transform cam;

    public bool reloaded, canShoot;
    public int bulletPos;
    public Animator[] animators;
    public TextMeshProUGUI ammo;
    public GameManager gameManager;

    void Start()
    {
        ammo.text = "0/6";
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloaded)
        {
            animators[0].Play("ReloadRevolver");
            animators[1].Play("ReloadRevolverBullet");
            gameManager.BulletPos.Value = Random.Range(0, 6);
            gameManager.currentPos.Value = 0;
            reloaded = true;
            ammo.text = "1/6";
        }

        if (animators[0].GetCurrentAnimatorStateInfo(0).IsName("ReloadRevolver"))
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && IsOwner)
        {
            if (reloaded && canShoot)
            {
                if (bulletPos == gameManager.currentPos.Value)
                {
                    ShootServerRpc();

                    reloaded = false;
                    animators[0].Play("Shooting");
                    ammo.text = "0/6";
                }
                
                gameManager.currentPos.Value++;
                gameManager.currentPos.Value %= 6;
            }

            if (!animators[0].GetCurrentAnimatorStateInfo(0).IsName("ReloadRevolver"))
            {
                animators[0].Play("Triggering");
                animators[2].Play("TriggeringArm");
            }
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        
        bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<NetworkObject>().Spawn();

        if (bullet.TryGetComponent<Rigidbody>(out var bulletRigidbody))
        {
            Vector3 direction = cam.forward;

            bulletRigidbody.rotation = Quaternion.LookRotation(direction);
            bulletRigidbody.velocity = direction * bulletSpeed;
        }
    }
}