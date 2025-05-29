using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전 속도
    public float rotSpeed = 500f;

    //회전 값 변수 만들어줌
    float mx = 0; 
    float my = 0;

    // Start is called before the first frame update
    void Start()//스타트함수 -> 객체가 생겼을때 한번만 불려짐.
    {
           
    }

    // Update is called once per frame
    void Update()//업데이트 프레임이 업데이트 될때 마다 불려진다.
        //업데이트함수의 맹점은 컴퓨터의 사양에따라 불려지는 횟수가 다름. 
    {
        //1.사용자의 마우스 입력을 받아 물체를 회전시키고 싶다.
        //마우스의 입력을 받는 부분
        float mouse_X = Input.GetAxis("Mouse X");//여기서 Mouse X는 
        float mouse_Y = Input.GetAxis("Mouse Y");

        //1-1. 회전 값 변수에 마우스 입력 값만큼 누적시킴
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //1-2 마우스 상하이동 회전 변수(my)의 값을 -90, 90사이로 제한
        my = Mathf.Clamp(my, -90f, 90f);

        //2. 마우스 입력값을 이용해 회전 방향을 결정할꺼다.
        //이해안가면 직접 이변수를 바꿔가면서 느낌을 봐라
        Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);


        //회전 방향대로 물체를 회전시킨다.
        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

        //2.물체를 회전 방향으로 회전시킴
        transform.eulerAngles = new Vector3(-my, mx, 0);


        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90f, 90f);//clamp함수는 -90도에서 90도
        ////라고 한정시켜버린다. 
        //transform.eulerAngles = rot;

        //정면을 바라볼때를 0도, -1도만큼 회전해버리면 -1도가 아니라
        //유니티에서는 359도 
    }
}
