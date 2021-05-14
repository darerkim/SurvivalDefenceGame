using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //활성화 여부.
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun; //현재 소유하고 있는 총기

    //연사 속도
    private float currentFireRate;

    //효과음
    private AudioSource audioSource;

    //상태 변수
    private bool isReloading = false;
    [HideInInspector]
    public bool isFineSightMode = false;

    //본래의 포지션 값
    private Vector3 originPos;

    //레이저에 충돌한 객체의 정보를 받아옴
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();

        //WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        //WeaponManager.currentWeaponAnim = currentGun.anim;
    }

    void Update()
    {
        if (isActivate && !Inventory.inventoryActivated && !GameManager.isPreviewActivated)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    //정조준 시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReloading)
        {
            FineSight();
        }
    }

    //정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    //정조준
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        theCrosshair.FineSightAnimation(isFineSightMode);
        currentGun.anim.SetBool("FineSight", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutin());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutin());
        }

    }

    //정조준 활성화
    IEnumerator FineSightActivateCoroutin()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
        
    }
    //정조준 비활성화
    IEnumerator FineSightDeactivateCoroutin()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }

    }

    //재장전 시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            if (!isReloading)
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutin());
            }
        }
    }

    public void CancelReload()
    {
        if (isReloading)
        {
            StopAllCoroutines();
            isReloading = false;
        }
    }

    //연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    //발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReloading)
        {
            Fire();
        }
    }

    //발사 전 계ㅅ
    private void Fire()
    {
        if (!isReloading)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutin());
            } 
        }

    }

    //발사
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;             
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();
        //총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutin());

        Debug.Log("총알 발사함!");
    }

    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0)
            , out hitInfo, currentGun.range, layerMask))
        {
            Debug.Log(hitInfo.transform.name + "    확인");
            var clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            Destroy(clone, 1f);
        }
    }

    //반동 코루틴
    IEnumerator RetroActionCoroutin()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }

    }

    //재장전
    IEnumerator ReloadCoroutin()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReloading = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);    

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReloading = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    //효과음 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;

        //GetComponent<Transform> -> transform
        WeaponManager.currentWeapon = currentGun.transform;
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);

        isActivate = true;
    }
}
