using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //폭발 이펙트 프리팹 변수
    //이펙트 넣어줌.
    public GameObject bombEffect;

    //충돌했을때의 처리
    private void OnCollisionEnter(Collision collision)
    {
        //이펙트 프리팹을 생성한다.
        GameObject eff = Instantiate(bombEffect);
        //이펙트 프리팹의 위치는 수류탄 오브젝트 자신의 위치와 동일하다.
        eff.transform.position = transform.position;

        Destroy(gameObject);//파괴한다. gameObject는 이 스크립트를 가지고 있는 오브젝트를 이야기한다.
    }
}
