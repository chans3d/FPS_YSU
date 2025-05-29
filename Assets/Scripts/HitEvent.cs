using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    //에너미 스크립트 컴포넌트를 사용하기 위한 변수
    public EnemyFSM efsm;

    public void PlayerHit()
    {
        efsm.AttackAction();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
