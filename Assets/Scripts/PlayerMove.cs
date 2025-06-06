using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //�ִϸ����� ����
    Animator anim;
    
    //�̵��ӵ� ����
    public float moveSpeed = 7f;

    //ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    //�߷º���
    float gravity = -20f;

    //���� �ӷº���
    float yVelocity = 0;

    //������ �ϱ����� ����
    public float jumpPower = 10f;

    //���� ���� ����
    public bool isJumping = false; //bool���� true(��)�ƴϸ� false(����)
                                   //������ �غ��� ���? �ǰ��� �� �� �ִ°�?
                                   //1. �����̽��ٸ� ������ true

    //�÷��̾��� ü�º���
    public int hp = 20;
    //�ִ� ü�� ����
    int maxHp = 20;
    //hp �����̴� ���� -> �巡���ؼ� �־�����ϴ� �κ�..
    public Slider hpSlider;

    private void Start()
    {
        cc = GetComponent<CharacterController>();//ĳ���� ��Ʈ�ѷ��� ������. 
        //GetComponenet�� ������Ʈ�� ������ �ִ� ������Ʈ�� �����ϱ����ؼ� ������ �������� ���� ����. 
        //����..

        //�ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���°� '���� ��' ������ ���� ������ �� �ְ� �Ѵ�.
        if(GameManager.gm.gameState != GameManager.GameState.Run)
        {
            return;
        }

        //Ű���� W, A, S, D ��ư�� �Է��ϸ� ĳ���͸� �� �������� �̵���Ű�� ��.

        //1. ������� �Է¹ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //2. �̵������� �����ϱ�
        Vector3 dir = new Vector3(h, 0, v);
        
        dir = dir.normalized;

        //�̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ��ش�. 
        //�ᱹ���� �ִϸ������� �Ķ���Ϳ� ����
        anim.SetFloat("MoveMotion", dir.magnitude);


        //2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�. 
        //������ǥ�� -> ������ǥ�� �ٲ�����Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);
        //2-2���� ���� ���̰� �ٽ� �ٴڿ� ���� �ߴٸ�
        if (isJumping && cc.collisionFlags == CollisionFlags.Below) // �������̰� ���� �� ����������
        {
            //���� �� ���·� �ʱ�ȭ �Ѵ�.
            isJumping = false;
        }
        
        if (Input.GetButtonDown("Jump") && !isJumping) //������ư�� ������ isJumping�� �����϶� -> �Ѹ���� ǥ�����ڸ� �� �����ϴ� ����.
        {
            //ĳ���� ���� �ӵ��� �������� �����ϰ� �������·� �����Ѵ�.
            yVelocity = jumpPower;
            isJumping = true;
            yVelocity = 0;
        }

      
        //2-2 ĳ���� ���� �ӵ��� �߷°��� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        //3.�̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir*moveSpeed*Time.deltaTime);
        //3. �̵� �ӵ��� ���� �̵��Ѵ�.
        //p = p0 + vt
        //�̵��������� =  ���������� + ��ǲ��*�ӵ�*�ð�
        //transform.position += dir * moveSpeed * Time.deltaTime;

        //4. ���� �÷��̾� hp(%)�� hp�����̴��� value��  �ݿ�
        hpSlider.value = (float)hp / (float)maxHp;

    }


    //�÷��̾��� �ǰ� ������Ʈ
    public GameObject hitEffect;

    //�÷����� �ǰ� �Լ�, ���� hp���� ���� ���ݷ��� �����ϴ� ���� 
    public void DamageAction(int damage)
    {
        //���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ��´�.
        hp -= damage;
        //����, �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ���� ����Ѵ�. 
        if(hp>0)
        {
            //�ǰ� ����Ʈ �ڷ�ƾ�� �����Ѵ�. 
            StartCoroutine(PlayHitEffect());
        }
    }
    //�ǰ�ȿ�� �ڷ�ƾ �Լ�
    IEnumerator PlayHitEffect()
    {
        //1.�ǰ� UI�� Ȱ��ȭ�Ѵ�. 
        hitEffect.SetActive(true);
        //2. 0.3�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(0.3f);
        //3.�ǰ� UI�� ��Ȱ��ȭ�Ѵ�.
        hitEffect.SetActive(false);
    }



}
