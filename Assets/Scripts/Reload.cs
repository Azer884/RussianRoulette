using TMPro;
using UnityEngine;

public class Reload : MonoBehaviour
{
    [HideInInspector]public bool reloaded, canShoot;
    [HideInInspector]public int bulletPos;
    public Animator[] animators;
    public TextMeshProUGUI ammo;
    void Start()
    {
        ammo.text = "0/6";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloaded)
        {
            animators[0].Play("ReloadRevolver");
            animators[1].Play("ReloadRevolverBullet");
            bulletPos = Random.Range(0, 6);
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
    }
}
