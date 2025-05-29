using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //ȸ�� �ӵ�
    public float rotSpeed = 500f;

    //ȸ�� �� ���� �������
    float mx = 0; 
    float my = 0;

    // Start is called before the first frame update
    void Start()//��ŸƮ�Լ� -> ��ü�� �������� �ѹ��� �ҷ���.
    {
           
    }

    // Update is called once per frame
    void Update()//������Ʈ �������� ������Ʈ �ɶ� ���� �ҷ�����.
        //������Ʈ�Լ��� ������ ��ǻ���� ��翡���� �ҷ����� Ƚ���� �ٸ�. 
    {
        //1.������� ���콺 �Է��� �޾� ��ü�� ȸ����Ű�� �ʹ�.
        //���콺�� �Է��� �޴� �κ�
        float mouse_X = Input.GetAxis("Mouse X");//���⼭ Mouse X�� 
        float mouse_Y = Input.GetAxis("Mouse Y");

        //1-1. ȸ�� �� ������ ���콺 �Է� ����ŭ ������Ŵ
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //1-2 ���콺 �����̵� ȸ�� ����(my)�� ���� -90, 90���̷� ����
        my = Mathf.Clamp(my, -90f, 90f);

        //2. ���콺 �Է°��� �̿��� ȸ�� ������ �����Ҳ���.
        //���ؾȰ��� ���� �̺����� �ٲ㰡�鼭 ������ ����
        Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);


        //ȸ�� ������ ��ü�� ȸ����Ų��.
        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

        //2.��ü�� ȸ�� �������� ȸ����Ŵ
        transform.eulerAngles = new Vector3(-my, mx, 0);


        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90f, 90f);//clamp�Լ��� -90������ 90��
        ////��� �������ѹ�����. 
        //transform.eulerAngles = rot;

        //������ �ٶ󺼶��� 0��, -1����ŭ ȸ���ع����� -1���� �ƴ϶�
        //����Ƽ������ 359�� 
    }
}
