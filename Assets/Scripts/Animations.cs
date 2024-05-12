using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class Animations : NetworkBehaviour
{
    public Animator animator ; 
    public float VelocityX = 0f;
    public float VelocityZ = 0f;
    public float Acceleration = 2f;
    public float Decceleration = 2f;
    

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            bool Forward = Input.GetKey(KeyCode.W);
            bool Backward = Input.GetKey(KeyCode.S);
            bool Rightward = Input.GetKey(KeyCode.D);
            bool Leftward = Input.GetKey(KeyCode.A);
            bool Shift = Input.GetKey(KeyCode.LeftShift);


            if (Forward && VelocityZ < 0.5f && !Shift)
            {
                VelocityZ += Time.deltaTime * Acceleration ;
            }
            if (Rightward && VelocityX < 0.5f)
            {
                VelocityX += Time.deltaTime * Acceleration ;
            }


            if (Backward && VelocityZ > -0.5f)
            {
                VelocityZ -= Time.deltaTime * Acceleration ;
            }
            if (Leftward && VelocityX > -0.5f)
            {
                VelocityX -= Time.deltaTime * Acceleration ;
            }
        
        
            //DECCELARATION

            if (!Forward && VelocityZ > 0f)
            {
                VelocityZ -= Time.deltaTime * Decceleration ;
            }
            if (!Rightward && VelocityX > 0f)
            {
                VelocityX -= Time.deltaTime * Decceleration ;
            }

            if (!Leftward && VelocityX < 0f)
            {
                VelocityX += Time.deltaTime * Decceleration ;
            }
            if (!Backward && VelocityZ < 0f)
            {
                VelocityZ += Time.deltaTime * Decceleration ;
            }
        

            if (Forward && VelocityZ < 1f && Shift)
            {
                VelocityZ += Time.deltaTime * Acceleration ;
            }
            if (Forward && VelocityZ > 0.5f && !Shift)
            {
                VelocityZ -= Time.deltaTime * Decceleration ;
            }

            animator.SetFloat("Velocity X", VelocityX);
            animator.SetFloat("Velocity Z", VelocityZ);
        }
    }
    
}

