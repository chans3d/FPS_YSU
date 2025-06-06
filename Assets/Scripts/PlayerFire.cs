using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //애니메이터 변수
    Animator anim;

    //발사 위치
    public GameObject firePosition;
    //투척 무기 오브젝트
    public GameObject bombFactory;

    //투척 파워
    public float throwPower = 15f;//15만큼의 데미지를 준다.

    //피격 이펙트 오브젝트
    public GameObject bulletEffect;
    //피격 이펙트 파티클 시스템
    ParticleSystem ps;

    //발사 무기 공격력

    public int weaponPower = 5;
    //유니티에서 public을 쓰는 이유
    //1.아무생각없이 에디터에서 보기위해서..
    //2.다른 스크립트에서 참조하기 위해서..
    

    // Start is called before the first frame update
    private void Start()
    {
        //피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();

        //애니메이터 컴포넌트 가져오기
        anim = GetComponentInChildren<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        //마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.    
        if(Input.GetMouseButtonDown(1))
        {
            //수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 한다.
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            //수류탄 오브젝트의 RigidBody컴포넌트를 가져온다.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //카메라의 방향으로 수류탄을 물리적 힘을 가한다.
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        if(Input.GetMouseButtonDown(0)) //0번이 마우스 왼쪽클릭
        {
            //만일 이동 블랜드 트리 파라미터의 값이 0이라면, 공격애니메이션을 실시. 
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            //레이를 생성한 후 발사될 위치와 진행방향을 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            //RaycastHit은 구조체인데, 어떤 정보를 가지냐면, Ray에 부딪힌 오브젝트의 정보를 가진다.
            //충돌한 좌표를 가져올수 있고, 충돌한 지점의 법선 벡터, 폴리곤의 위치, 거리도 젤수 있음
            //몇가지의 컴포넌트들을 가져올 수 있다.
            RaycastHit hitInfo = new RaycastHit();
            //레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트를 표시한다.
            if(Physics.Raycast(ray, out hitInfo)) 
            {
                //만일 레이어부딪힌 대상의 레이어가 'Enemy'라면 데미지 함수를 실행한다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();//EnemyFSM의 컴포넌트 가져오기
                    eFSM.HitEnemy(weaponPower);//총알의 공격력을 넣어주는 것 함수호출
                }//그렇지 않다면 레이어 부딪힌 지점에 피격 이펙트를 플레이한다.
                else
                {
                    //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffect.transform.position = hitInfo.point;
                    //피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다. 
                    bulletEffect.transform.forward = hitInfo.normal;
                    //피격이펙트를 플레이한다.
                    ps.Play();
                }
            }
        }
    }
}
