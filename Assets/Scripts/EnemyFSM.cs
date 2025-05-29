using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//이거 추가. 

public class EnemyFSM : MonoBehaviour
{
   //에너미 상태 상수
   enum EnemyState
    {
        Idle,//0 
        Move,//1
        Attack,//2
        Return,//3
        Damaged,//4
        Die//5
    }

    //에너미 상태 변수
    EnemyState m_State;
    //플레이어를 발견하는 범위
    public float findDistance = 8f;
    //플레이어의 위치
    Transform player;

    //공격 가능 범위
    public float attackDistance = 2f;
    //이동속도
    public float moveSpeed = 5f;
    //캐릭터 콘트롤러 컴포넌트
    CharacterController cc;

    //누적시간
    float currentTime = 0;
    //공격 딜레이시간
    float attackDelay = 2f;
    //에너미 공격력
    public int AttackPower = 3;

    //초기 위치 저장용 변수
    Vector3 originPos;
   
    Quaternion originRot;


    //이동 가능 범위
    public float moveDistance = 20f;

    Animator anim;


    private void Start()
    {
        //최초의 에너미 상태는 대기로 합니다 .
        m_State = EnemyState.Idle;//대기
        //플레이어의 트랜스폼 컴포넌트를 받아옴.
        player = GameObject.Find("Player").transform;
        //캐릭터 콘트롤러를 받아와야함
        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        //자식 오브젝트로부터 애니메이터 변수 받아오기
        anim = transform.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //현재 hp(%)를 hp슬라이더의 value에 반영한다.
        hpSlider.value = (float)hp / (float)maxHp;

        //에너미 상태 설정
        //현재 상태를 체크해서 해당 상태별로 정해진 기능을 수행하고 싶다. 
        switch (m_State) //0,1,2,3,4 
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

    }
    void Idle()
    {
        //만일 플레이어와의 거리가 액션 시작 범위 이내라면 Move상태로 전환한다.
        //findDistance가 8이었기때문에 8보다 작다면..
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("상태전환: Idle -> Move");
            
            //이동 애니메이션으로 전환하기.
            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //만일 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면...20m보다 크다면...
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            //현재 상태를 복귀(Return)으로 바꿔줌. 
            m_State = EnemyState.Return;
            Debug.Log("상태 전환: Move -> Return");
        }else if(Vector3.Distance(transform.position, player.position) > attackDistance)//만일, 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다.
        {
            //이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;
            //캐릭터 컨트롤러를 이용해서 이동하기
            //p(이동할 포지션) = p0(현재나의위치)+ v(스피드)*t(시간)
            cc.Move(dir*moveSpeed*Time.deltaTime);

            //플레이어를 향해 방향 전환한다. 
            transform.forward = dir;


        }//그렇지 않다면 현재 상태는 공격으로 바꿔줌..
        else
        {
            m_State = EnemyState.Attack;
            Debug.Log("상태 전환: Move -> Attack");

            //누적시간을 공격 딜레이 시간 만큼 미리 진행시켜 놓는다.
            currentTime = attackDelay;
            //공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    //플레이어의 스크립트의 데미지 처리 함수를 실행하기
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(AttackPower);
    }

    void Attack()
    {
        //만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격한다.
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;//currentTime 0의값을 가지고 있었고, 
            //그값을 증가시켜주는 역할
            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(AttackPower);
                Debug.Log("공격");
                currentTime = 0;//초기화를 해줌. 

                //공격 애니메이션 플레이
                anim.SetTrigger("StartAttack");
            }
        }else //그렇지 않다면 현재 상태를 이동(Move)으로 전환한다.(재추격실시)
        {
            m_State = EnemyState.Move;
            Debug.Log("상태 전환: Attack -> Move");
            currentTime = 0;

            //이동 애니메이션 플레이
            anim.SetTrigger("AttackToMove");
        }
    }
    public int hp =15 ;
    public int maxHp = 15;
    //에너미 hp Slider 변수 
    public Slider hpSlider;
    //데미지 실행 함수를 구현
    public void HitEnemy(int hitPower)
    {
        //만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        //플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;
        //에너미의 체력이 0보다 크면 피격상태로 전환한다.
        if(hp>0)
        {
            m_State = EnemyState.Damaged; //상태전환
            Debug.Log("상태전환: Any state -> Damaged");
            //피격 애니메이션을 플레이한다. 
            anim.SetTrigger("Damaged");

            Damaged();//함수를 호출
        }//그렇지않다면 -> 0보다 작을때
        else
        {
            m_State = EnemyState.Die;
            Debug.Log("상태전환: Any state -> Die");

            //죽음애니메이션을 플레이한다. 
            anim.SetTrigger("Die");

            Die();
        }
    }

    void Return()
    {
        //만일 초기 위치에서 거리가 0.1f 이상이라면 초기 위치쪽으로 이동한다. 
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            //복귀 지점으로 방향을 전환한다. 
            transform.forward = dir;

        }//그렇지않다면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
        else
        {
            //위치 값과 회전 값을 초기 상태로 변환한다. 
            transform.position = originPos;
            transform.rotation = originRot;
            //hp를 다시 회복한다.
            hp = maxHp;
            m_State = EnemyState.Idle;
            Debug.Log("상태전환: Return->Idle");
            //대기 애니메이션으로 전환하는 트랜지션 호출
            anim.SetTrigger("MoveToIdle");

        }


    }

    void Damaged()
    {
        //피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamagedProcess());
    }
    void Die()//죽었을때 
    {
        //이미 진행중인 피격 코루틴이 사망상태에서 안되어야하는 상황...
        //코루틴의 제거..
        //진행중인 코루틴을 중지
        StopAllCoroutines();

        //죽음 상태로 처리하기 위한 코루틴을 실행
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        //캐릭터 컨트롤러 컴포넌트를 비활성화시킨다.
        cc.enabled = false;
        //2초동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        Debug.Log("소멸!");
        Destroy(gameObject);
    }
    //데미지 처리용 코루틴 함수
    IEnumerator DamagedProcess()
    {
        //피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(1.0f);

        //현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        Debug.Log("상태전환: Damaged -> Move");
    }
}
