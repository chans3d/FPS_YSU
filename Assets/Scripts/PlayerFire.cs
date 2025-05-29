using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //�ִϸ����� ����
    Animator anim;

    //�߻� ��ġ
    public GameObject firePosition;
    //��ô ���� ������Ʈ
    public GameObject bombFactory;

    //��ô �Ŀ�
    public float throwPower = 15f;//15��ŭ�� �������� �ش�.

    //�ǰ� ����Ʈ ������Ʈ
    public GameObject bulletEffect;
    //�ǰ� ����Ʈ ��ƼŬ �ý���
    ParticleSystem ps;

    //�߻� ���� ���ݷ�

    public int weaponPower = 5;
    //����Ƽ���� public�� ���� ����
    //1.�ƹ��������� �����Ϳ��� �������ؼ�..
    //2.�ٸ� ��ũ��Ʈ���� �����ϱ� ���ؼ�..
    

    // Start is called before the first frame update
    private void Start()
    {
        //�ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();

        //�ִϸ����� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        //���콺 ������ ��ư�� ������ �ü��� �ٶ󺸴� �������� ����ź�� ������ �ʹ�.    
        if(Input.GetMouseButtonDown(1))
        {
            //����ź ������Ʈ�� ������ �� ����ź�� ���� ��ġ�� �߻� ��ġ�� �Ѵ�.
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            //����ź ������Ʈ�� RigidBody������Ʈ�� �����´�.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //ī�޶��� �������� ����ź�� ������ ���� ���Ѵ�.
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        if(Input.GetMouseButtonDown(0)) //0���� ���콺 ����Ŭ��
        {
            //���� �̵� ���� Ʈ�� �Ķ������ ���� 0�̶��, ���ݾִϸ��̼��� �ǽ�. 
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            //���̸� ������ �� �߻�� ��ġ�� ��������� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //���̰� �ε��� ����� ������ ������ ������ �����Ѵ�.
            //RaycastHit�� ����ü�ε�, � ������ �����ĸ�, Ray�� �ε��� ������Ʈ�� ������ ������.
            //�浹�� ��ǥ�� �����ü� �ְ�, �浹�� ������ ���� ����, �������� ��ġ, �Ÿ��� ���� ����
            //����� ������Ʈ���� ������ �� �ִ�.
            RaycastHit hitInfo = new RaycastHit();
            //���̸� �߻��� �� ���� �ε��� ��ü�� ������ �ǰ� ����Ʈ�� ǥ���Ѵ�.
            if(Physics.Raycast(ray, out hitInfo)) 
            {
                //���� ���̾�ε��� ����� ���̾ 'Enemy'��� ������ �Լ��� �����Ѵ�.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();//EnemyFSM�� ������Ʈ ��������
                    eFSM.HitEnemy(weaponPower);//�Ѿ��� ���ݷ��� �־��ִ� �� �Լ�ȣ��
                }//�׷��� �ʴٸ� ���̾� �ε��� ������ �ǰ� ����Ʈ�� �÷����Ѵ�.
                else
                {
                    //�ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵���Ų��.
                    bulletEffect.transform.position = hitInfo.point;
                    //�ǰ� ����Ʈ�� forward ������ ���̰� �ε��� ������ ���� ���Ϳ� ��ġ��Ų��. 
                    bulletEffect.transform.forward = hitInfo.normal;
                    //�ǰ�����Ʈ�� �÷����Ѵ�.
                    ps.Play();
                }
            }
        }
    }
}
