using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    ///활성화 여부.
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutin()
    {
        if (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.CompareTag("Rock"))
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.CompareTag("WeakAnimal"))
                {
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                    SoundManager.instance.PlaySE("Animal_Hit");
                }
                isSwing = false;
                Debug.Log(hitInfo.transform.name);

            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
