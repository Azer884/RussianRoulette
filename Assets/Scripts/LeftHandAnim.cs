using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeftHandAnim : MonoBehaviour
{
    public TwoBoneIKConstraint rig;
    public Animator armAnim;
    private float weightValue = 1;

    void Update()
    {
        rig.weight = weightValue;
        if (weightValue == 0f && !armAnim.GetCurrentAnimatorStateInfo(0).IsName("RollRevolver"))
        {
            armAnim.Play("RollRevolver");
        }
    }
    public void ArmMovement(float index)
    {
        weightValue = index;
    }
}
