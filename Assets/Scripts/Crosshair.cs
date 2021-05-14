using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    [SerializeField]
    private Animator anim;

    //크로스헤어에 따른 총의 정확도
    private float gunAccuracy;

    //크로스헤어 활성화/비활성화를 위한 부모객
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
            anim.SetBool("Walking", _flag);
        }
    }

    public void RunningAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
            anim.SetBool("Running", _flag);
        }
    }

    public void JumpingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            anim.SetBool("Running", _flag);
        } 
    }

    public void CrouchingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            anim.SetBool("Crouching", _flag);
        }
    }

    public void FineSightAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            anim.SetBool("FineSight", _flag);
        }
    }

    public void FireAnimation()
    {
        if (!GameManager.isWater)
        {
            if (anim.GetBool("Walking"))
                anim.SetTrigger("Walk_Fire");
            else if (anim.GetBool("Crouching"))
                anim.SetTrigger("Crouch_Fire");
            else
                anim.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking"))
            gunAccuracy = 0.06f;
        else if (anim.GetBool("Crouching"))
            gunAccuracy = 0.015f;
        else if (theGunController.isFineSightMode)
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;

        return gunAccuracy;
    }
}
