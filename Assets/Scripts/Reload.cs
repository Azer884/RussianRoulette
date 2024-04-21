using UnityEngine;

public class Reload : MonoBehaviour
{
    public bool reloaded, canShoot;
    public int bulletPos;
    public Animator[] animators;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloaded)
        {
            animators[0].Play("ReloadRevolver");
            animators[1].Play("ReloadRevolverBullet");
            bulletPos = Random.Range(0, 6);
            reloaded = true;
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
