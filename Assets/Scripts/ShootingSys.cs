using System.Collections;
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

    public bool canShoot;
    public int bulletPos;
    public Animator[] animators;
    public TextMeshProUGUI ammo;
    public GameManager gameManager;

    void Start()
    {
        ammo.text = "0/6";
        if (IsClient)
        {
            StartCoroutine(FindGameManager());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !gameManager.netReloaded.Value)
        {
            animators[0].Play("ReloadRevolver");
            animators[1].Play("ReloadRevolverBullet");
            gameManager.bulletPosition = Random.Range(0, 6);
            gameManager.currentPosition = 0;
            gameManager.reloaded = true;
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
            if (gameManager.netReloaded.Value && canShoot)
            {
                if (bulletPos == gameManager.currentPosition)
                {
                    ShootServerRpc();

                    gameManager.reloaded = false;
                    animators[0].Play("Shooting");
                    ammo.text = "0/6";
                }
                
                gameManager.currentPosition++;
                gameManager.currentPosition %= 6;
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

    private IEnumerator FindGameManager()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        gameManager = GameManager.Instance;
    }
}