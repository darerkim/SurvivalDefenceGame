using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//미완성 클래스 = 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
    //현재 장착된 핸드형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //공격중인지 판단
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    [SerializeField] protected LayerMask layerMask; 

    protected void TryAttack()
    {
        if (!GameManager.isOpenInventory && !GameManager.isPreviewActivated)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutin());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutin()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutin());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    //미완성 클래스 = 추상 클래스
    protected abstract IEnumerator HitCoroutin();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
            return true;
        return false;
    }

    //가상 함수 : 완성 함수이지만 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;

        //GetComponent<Transform> -> transform
        WeaponManager.currentWeapon = currentCloseWeapon.transform;
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
