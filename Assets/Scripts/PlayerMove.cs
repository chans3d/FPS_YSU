using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //애니메이터 변수
    Animator anim;
    
    //이동속도 변수
    public float moveSpeed = 7f;

    //캐릭터 컨트롤러 변수
    CharacterController cc;

    //중력변수
    float gravity = -20f;

    //수직 속력변수
    float yVelocity = 0;

    //점프를 하기위한 변수
    public float jumpPower = 10f;

    //점프 상태 변수
    public bool isJumping = false; //bool값은 true(참)아니면 false(거짓)
                                   //예측을 해보면 어떨때? 판가름 할 수 있는가?
                                   //1. 스페이스바를 누르면 true

    //플레이어의 체력변수
    public int hp = 20;
    //최대 체력 변수
    int maxHp = 20;
    //hp 슬라이더 변수 -> 드래그해서 넣어줘야하는 부분..
    public Slider hpSlider;

    private void Start()
    {
        cc = GetComponent<CharacterController>();//캐릭터 컨트롤러를 가져옴. 
        //GetComponenet는 오브젝트가 가지고 있는 컴포넌트에 접근하기위해서 정보를 가져오는 것을 말함. 
        //참조..

        //애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if(GameManager.gm.gameState != GameManager.GameState.Run)
        {
            return;
        }

        //키보드 W, A, S, D 버튼을 입력하면 캐릭터를 그 방향으로 이동시키는 것.

        //1. 사용자의 입력받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //2. 이동방향을 설정하기
        Vector3 dir = new Vector3(h, 0, v);
        
        dir = dir.normalized;

        //이동 블렌딩 트리를 호출하고 벡터의 크기 값을 넘겨준다. 
        //결국에는 애니메이터의 파라미터에 접근
        anim.SetFloat("MoveMotion", dir.magnitude);


        //2-1. 메인 카메라를 기준으로 방향을 반환한다. 
        //로컬좌표를 -> 월드좌표로 바꿔줘야한다.
        dir = Camera.main.transform.TransformDirection(dir);
        //2-2만일 점프 중이고 다시 바닥에 착지 했다면
        if (isJumping && cc.collisionFlags == CollisionFlags.Below) // 점프중이고 이제 막 착지했을때
        {
            //점프 전 상태로 초기화 한다.
            isJumping = false;
        }
        
        if (Input.GetButtonDown("Jump") && !isJumping) //점프버튼을 누르고 isJumping이 거짓일때 -> 한마디로 표현하자면 막 점프하는 시점.
        {
            //캐릭터 수직 속도에 점프력을 적용하고 점프상태로 변경한다.
            yVelocity = jumpPower;
            isJumping = true;
            yVelocity = 0;
        }

      
        //2-2 캐릭터 수직 속도에 중력값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        //3.이동 속도에 맞춰 이동한다.
        cc.Move(dir*moveSpeed*Time.deltaTime);
        //3. 이동 속도에 맞춰 이동한다.
        //p = p0 + vt
        //이동할포지션 =  현재포지션 + 인풋값*속도*시간
        //transform.position += dir * moveSpeed * Time.deltaTime;

        //4. 현재 플레이어 hp(%)를 hp슬라이더의 value에  반영
        hpSlider.value = (float)hp / (float)maxHp;

    }


    //플레이어의 피격 오브젝트
    public GameObject hitEffect;

    //플레이의 피격 함수, 나의 hp에서 적의 공격력을 차감하는 형태 
    public void DamageAction(int damage)
    {
        //에너미의 공격력만큼 플레이어의 체력을 깍는다.
        hp -= damage;
        //만일, 플레이어의 체력이 0보다 크면 피격 효과를 출력한다. 
        if(hp>0)
        {
            //피격 이펙트 코루틴을 시작한다. 
            StartCoroutine(PlayHitEffect());
        }
    }
    //피격효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        //1.피격 UI를 활성화한다. 
        hitEffect.SetActive(true);
        //2. 0.3초간 대기한다.
        yield return new WaitForSeconds(0.3f);
        //3.피격 UI를 비활성화한다.
        hitEffect.SetActive(false);
    }



}
