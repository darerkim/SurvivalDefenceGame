using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; //동물의 이름.
    [SerializeField] protected int hp; //동물의 체력.
    [SerializeField] protected float walkSpeed; //걷는 속도.
    [SerializeField] protected float runSpeed; //뛰기 속도.
    [SerializeField] protected float turningSpeed; //회전 속도
    //protected float applySpeed;

    [SerializeField] protected float walkTime; //걷기 시간
    [SerializeField] protected float runTime;
    [SerializeField] protected float waitTime; //대기 시간

    protected float currentTime;
    //protected Vector3 direction; //방향
    protected Vector3 destination; //목적

    //상태변수
    protected bool isWalking; //걷는지 안걷는지 판별
    protected bool isRunning; //뛰는지 안뛰는지 판별
    protected bool isAction; //행동중인지 아닌지 판별
    protected bool isDead; //죽었는지 판별.

    //필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] sound_normal;
    [SerializeField] protected AudioClip sound_hurt;
    [SerializeField] protected AudioClip sound_dead;
    protected NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (!isDead)
        {
            Move();
            //Rotation();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            nav.SetDestination(transform.position + destination * 5f);
     }

    //protected void Rotation()
    //{
    //    if (isWalking || isRunning)
    //    {
    //        Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
    //        rigid.MoveRotation(Quaternion.Euler(_rotation));
    //    }
    //}

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime; 
            if (currentTime <= 0)
                Reset();//다음 렌덤 행동 개시
        }
    }

    protected virtual void Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        nav.speed = walkSpeed;
        //direction.Set(0f, Random.Range(0f, 360f), 0f);
        destination.Set(Random.Range(-0.2f, 0.2f), 0, Random.Range(0.5f, 1f));
        nav.ResetPath();
    }

    protected void TryWalk()
    {
        isWalking = true;
        currentTime = walkTime;
        nav.speed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        Debug.Log("걷기");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }
            anim.SetTrigger("Hurt");
            PlaySE(sound_hurt);
        }
    }

    protected void Dead()
    {
        PlaySE(sound_dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(sound_normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
