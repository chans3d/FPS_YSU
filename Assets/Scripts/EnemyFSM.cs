using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//�̰� �߰�. 

public class EnemyFSM : MonoBehaviour
{
   //���ʹ� ���� ���
   enum EnemyState
    {
        Idle,//0 
        Move,//1
        Attack,//2
        Return,//3
        Damaged,//4
        Die//5
    }

    //���ʹ� ���� ����
    EnemyState m_State;
    //�÷��̾ �߰��ϴ� ����
    public float findDistance = 8f;
    //�÷��̾��� ��ġ
    Transform player;

    //���� ���� ����
    public float attackDistance = 2f;
    //�̵��ӵ�
    public float moveSpeed = 5f;
    //ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    //�����ð�
    float currentTime = 0;
    //���� �����̽ð�
    float attackDelay = 2f;
    //���ʹ� ���ݷ�
    public int AttackPower = 3;

    //�ʱ� ��ġ ����� ����
    Vector3 originPos;
   
    Quaternion originRot;


    //�̵� ���� ����
    public float moveDistance = 20f;

    Animator anim;


    private void Start()
    {
        //������ ���ʹ� ���´� ���� �մϴ� .
        m_State = EnemyState.Idle;//���
        //�÷��̾��� Ʈ������ ������Ʈ�� �޾ƿ�.
        player = GameObject.Find("Player").transform;
        //ĳ���� ��Ʈ�ѷ��� �޾ƿ;���
        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        //�ڽ� ������Ʈ�κ��� �ִϸ����� ���� �޾ƿ���
        anim = transform.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //���� hp(%)�� hp�����̴��� value�� �ݿ��Ѵ�.
        hpSlider.value = (float)hp / (float)maxHp;

        //���ʹ� ���� ����
        //���� ���¸� üũ�ؼ� �ش� ���º��� ������ ����� �����ϰ� �ʹ�. 
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
        //���� �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move���·� ��ȯ�Ѵ�.
        //findDistance�� 8�̾��⶧���� 8���� �۴ٸ�..
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("������ȯ: Idle -> Move");
            
            //�̵� �ִϸ��̼����� ��ȯ�ϱ�.
            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //���� ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�...20m���� ũ�ٸ�...
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            //���� ���¸� ����(Return)���� �ٲ���. 
            m_State = EnemyState.Return;
            Debug.Log("���� ��ȯ: Move -> Return");
        }else if(Vector3.Distance(transform.position, player.position) > attackDistance)//����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�.
        {
            //�̵� ���� ����
            Vector3 dir = (player.position - transform.position).normalized;
            //ĳ���� ��Ʈ�ѷ��� �̿��ؼ� �̵��ϱ�
            //p(�̵��� ������) = p0(���糪����ġ)+ v(���ǵ�)*t(�ð�)
            cc.Move(dir*moveSpeed*Time.deltaTime);

            //�÷��̾ ���� ���� ��ȯ�Ѵ�. 
            transform.forward = dir;


        }//�׷��� �ʴٸ� ���� ���´� �������� �ٲ���..
        else
        {
            m_State = EnemyState.Attack;
            Debug.Log("���� ��ȯ: Move -> Attack");

            //�����ð��� ���� ������ �ð� ��ŭ �̸� ������� ���´�.
            currentTime = attackDelay;
            //���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    //�÷��̾��� ��ũ��Ʈ�� ������ ó�� �Լ��� �����ϱ�
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(AttackPower);
    }

    void Attack()
    {
        //����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ �����Ѵ�.
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;//currentTime 0�ǰ��� ������ �־���, 
            //�װ��� ���������ִ� ����
            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(AttackPower);
                Debug.Log("����");
                currentTime = 0;//�ʱ�ȭ�� ����. 

                //���� �ִϸ��̼� �÷���
                anim.SetTrigger("StartAttack");
            }
        }else //�׷��� �ʴٸ� ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ�.(���߰ݽǽ�)
        {
            m_State = EnemyState.Move;
            Debug.Log("���� ��ȯ: Attack -> Move");
            currentTime = 0;

            //�̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
        }
    }
    public int hp =15 ;
    public int maxHp = 15;
    //���ʹ� hp Slider ���� 
    public Slider hpSlider;
    //������ ���� �Լ��� ����
    public void HitEnemy(int hitPower)
    {
        //����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        //�÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;
        //���ʹ��� ü���� 0���� ũ�� �ǰݻ��·� ��ȯ�Ѵ�.
        if(hp>0)
        {
            m_State = EnemyState.Damaged; //������ȯ
            Debug.Log("������ȯ: Any state -> Damaged");
            //�ǰ� �ִϸ��̼��� �÷����Ѵ�. 
            anim.SetTrigger("Damaged");

            Damaged();//�Լ��� ȣ��
        }//�׷����ʴٸ� -> 0���� ������
        else
        {
            m_State = EnemyState.Die;
            Debug.Log("������ȯ: Any state -> Die");

            //�����ִϸ��̼��� �÷����Ѵ�. 
            anim.SetTrigger("Die");

            Die();
        }
    }

    void Return()
    {
        //���� �ʱ� ��ġ���� �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ������ �̵��Ѵ�. 
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            //���� �������� ������ ��ȯ�Ѵ�. 
            transform.forward = dir;

        }//�׷����ʴٸ� �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
        else
        {
            //��ġ ���� ȸ�� ���� �ʱ� ���·� ��ȯ�Ѵ�. 
            transform.position = originPos;
            transform.rotation = originRot;
            //hp�� �ٽ� ȸ���Ѵ�.
            hp = maxHp;
            m_State = EnemyState.Idle;
            Debug.Log("������ȯ: Return->Idle");
            //��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ������ ȣ��
            anim.SetTrigger("MoveToIdle");

        }


    }

    void Damaged()
    {
        //�ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamagedProcess());
    }
    void Die()//�׾����� 
    {
        //�̹� �������� �ǰ� �ڷ�ƾ�� ������¿��� �ȵǾ���ϴ� ��Ȳ...
        //�ڷ�ƾ�� ����..
        //�������� �ڷ�ƾ�� ����
        StopAllCoroutines();

        //���� ���·� ó���ϱ� ���� �ڷ�ƾ�� ����
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ��Ų��.
        cc.enabled = false;
        //2�ʵ��� ��ٸ� �Ŀ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);
        Debug.Log("�Ҹ�!");
        Destroy(gameObject);
    }
    //������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamagedProcess()
    {
        //�ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(1.0f);

        //���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        Debug.Log("������ȯ: Damaged -> Move");
    }
}
