using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //��ǥ�� �� Ʈ�������� ������Ʈ
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //ī�޶��� ��ġ�� ��ǥ Ʈ�������� ��ġ�� ��ġ��Ų��.
        transform.position = target.position;
    }
}
