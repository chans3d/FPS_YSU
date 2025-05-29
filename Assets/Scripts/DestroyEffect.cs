using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    //제거될 시간 변수
    public float destroyTime = 1.5f;
    //경과 시간 측정 변수
    float currentTime = 0; //시간 더해주는 역할

    // Update is called once per frame
    void Update()
    {
        //만일 경과 시간이 제거될 시간을 초과하면 자기 자신을 제거한다.
        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }
}
