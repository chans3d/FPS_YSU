using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //PlayerMove Ŭ���� ����
    PlayerMove player;

    //�̱���
    public static GameManager gm;//GameManager�� ����. 

    //������ ������ ���
    public enum GameState
    {
        Ready, //0
        Run,//1
        GameOver//2
    }
    //���� ���� UI ������Ʈ ����
    public GameObject gameLabel;//�����Ϳ��� �־���. 
    //���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText; //�ؽ�Ʈ ����� ���� ���� �־��� ����
    
    //������ ���� ���� ����
    public GameState gameState; 

    private void Awake()//��Ȱ��ȭ �Ǿ� �־ �����. 
    {
        if(gm==null)//gm�� �ƹ��͵� ���ٸ�....
        {
             gm = this;//�������� ����. 
        }

        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        //���� �ؽ�Ʈ�� ������ ��Ȳ������ �Ѵ�. 
        //�ؽ�Ʈ�� �÷��� ����
        gameText.color = new Color32(255, 185, 0, 255);

        //���� �غ� -> ���� �� ���·� ��ȯ�ϱ� Start�Լ�
        StartCoroutine(ReadyToStart());

    }
    IEnumerator ReadyToStart()
    {
        //2�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(2f);
        //���� �ؽ�Ʈ�� ������ 'Go!'�� �ٲ۴�.
        gameText.text = "Go!";
        //0.5�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(0.5f);

        //�����ؽ�Ʈ�� ��Ȱ��ȭ�Ѵ�.
        gameLabel.SetActive(false);
        //���¸� '���� ��' ���·� �����Ѵ�.
        gameState = GameState.Run;
        Debug.Log(gameState);
    }
    // Start is called before the first frame update
    void Start()
    {
        //�ʱ� ���� ���´� �غ���·� �����Ѵ�. 
        gameState = GameState.Ready;
        //�÷��̾� ������Ʈ�� ã�� �� �÷��̾��� PlayerMove������Ʈ �޾ƿ���
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� �÷��̾��� hp�� 0���϶��
        if(player.hp <= 0)
        {
            //���� �ؽ�Ʈ�� Ȱ��ȭ�Ѵ�
            gameLabel.SetActive(true);
            //���� �ؽ�Ʈ�� 'GameOver'�� ��ȯ�Ѵ�.
            gameText.text = "GameOver";

            //���� �ؽ�Ʈ�� ������ ���� ������ �Ѵ�.
            gameText.color = new Color32(255, 0, 0, 255);
            //���¸� '���� ����' ���·� �����Ѵ�.
            gameState = GameState.GameOver;
        }
    }
}
