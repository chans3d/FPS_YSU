using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //PlayerMove 클래스 변수
    PlayerMove player;

    //싱글톤
    public static GameManager gm;//GameManager가 들어옴. 

    //게임의 상태의 상수
    public enum GameState
    {
        Ready, //0
        Run,//1
        GameOver//2
    }
    //게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;//에디터에서 넣어줌. 
    //게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText; //텍스트 비워진 곳에 값을 넣어줄 변수
    
    //현재의 게임 상태 변수
    public GameState gameState; 

    private void Awake()//비활성화 되어 있어도 실행됨. 
    {
        if(gm==null)//gm이 아무것도 없다면....
        {
             gm = this;//동적으로 넣음. 
        }

        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        //상태 텍스트의 색상을 주황색으로 한다. 
        //텍스트의 컬러에 접근
        gameText.color = new Color32(255, 185, 0, 255);

        //게임 준비 -> 게임 중 상태로 전환하기 Start함수
        StartCoroutine(ReadyToStart());

    }
    IEnumerator ReadyToStart()
    {
        //2초간 대기한다.
        yield return new WaitForSeconds(2f);
        //상태 텍스트의 내용을 'Go!'로 바꾼다.
        gameText.text = "Go!";
        //0.5초간 대기한다.
        yield return new WaitForSeconds(0.5f);

        //상태텍스트를 비활성화한다.
        gameLabel.SetActive(false);
        //상태를 '게임 중' 상태로 변경한다.
        gameState = GameState.Run;
        Debug.Log(gameState);
    }
    // Start is called before the first frame update
    void Start()
    {
        //초기 게임 상태는 준비상태로 설정한다. 
        gameState = GameState.Ready;
        //플레이어 오브젝트를 찾은 후 플레이어의 PlayerMove컴포넌트 받아오기
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //만일 플레이어의 hp가 0이하라면
        if(player.hp <= 0)
        {
            //상태 텍스트를 활성화한다
            gameLabel.SetActive(true);
            //상태 텍스트를 'GameOver'로 전환한다.
            gameText.text = "GameOver";

            //상태 텍스트의 색상을 붉은 색으로 한다.
            gameText.color = new Color32(255, 0, 0, 255);
            //상태를 '게임 오버' 상태로 변경한다.
            gameState = GameState.GameOver;
        }
    }
}
