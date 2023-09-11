using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글턴 변수
    public static GameManager gm;

    void Awake()
    {
        if (gm == null) 
        {
            gm = this;        }        
    }

    //게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    //현재의 게임 상태 변수
    public GameState gState;

    //게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;
    //게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    //PlayerMove 클래스 변수
    PlayerMove player;

    //옵션 화면 UI 오브젝트 변수
    public GameObject gameOption;

    //애너미 오브젝트 배열
    GameObject[] enemys;

    //경과시간 저장 변수
    float time = 0f;

    //에너미 카운트 UI 오브젝트 변수
    public GameObject Enemy_C;
   
    //경과시간 UI 오브젝트 변수
    public GameObject time_C;
   

    //옵션 화면 켜기
    public void openOptionWindow()
    {
        // 옵션 창을 활성화한다.
        gameOption.SetActive(true);
        //게임 속도를 0배속으로 전환한다.
        Time.timeScale = 0f;
        // 게임 상태를 일시 정지 상태로 변경한다.
        gState = GameState.Pause;
        //마우스 활성화
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        //마우스 숨기기,비활성화
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // 옵션 창을 비활성화한다.
        gameOption.SetActive(false);
        //게임 속도를 1배속으로 전환.
        Time.timeScale = 1f;
        //게임 상태를 게임 중으로 변경
        gState = GameState.Run;

    }
    //다시하기 옵션
    public void RestartGame()
    {
        //게임 속도를 1배속으로 전환한다.
        Time.timeScale = 1f;
        //현재 씬 번호를 다시 로드한다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // 게임 종료 옵션
    public void QuitGame()
    {
        //애플리케이션 종료
        Application.Quit();
    }


    void Start()
    {
        //마우스 숨기기,비활성화
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //초기 게임 상태는 준비 상태로 설정한다.
        gState = GameState.Ready;
        //게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다.
        gameText = gameLabel.GetComponent<Text>();
        // 상태 텍스트의 내용을 'Ready'로 한다.
        gameText.text = "Ready";
        // 상태 텍스트의 색상을 주황색으로 한다.
        gameText.color = new Color32(255, 185, 0, 255);

        //게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());

        //플레이어 오브잭트를 찾은 후 플레이어의 PlayerMove컴포넌트 받아오기
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    IEnumerator ReadyToStart()
    {
        //2초간 대기한다
        yield return new WaitForSeconds(2f);
        //상태 텍스트의 내용을 'GO!'로 한다..
        gameText.text = "GO!";
        //0.5초간 대기한다
        yield return new WaitForSeconds(0.5f);
        //상태 택스트를 비활성화한다.
        gameLabel.SetActive(false);
        //'게임 중'상태로 변경
        gState = GameState.Run;
    }


    void Update()
    {
        //애너미 수 카운팅
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        
        if (gState == GameState.Run)
        {
            //애너미UI 출력
            Enemy_C.GetComponent<Text>().text = "남은 좀비:" + enemys.Length;
            //시간 카운팅
            time += Time.deltaTime;
            //시간UI 출력
            time_C.GetComponent<Text>().text = "경과 시간:" + ((int)time%60);
        }

        //만일, 좀비를 다 잡으면 클리어
        if (enemys.Length == 0)
        {
            //플레이어의 애니메이션을 멈춘다.
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            //상태 텍스트를 활성화한다.
            gameLabel.SetActive(true);
            //상태 텍스트내용'Mission Complete'로 변경
            gameText.text = "Mission Complete \r\n경과 시간:" + ((int)time % 60);
            //상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져온다.
            Transform buttons = gameText.transform.GetChild(0);
            //버튼 오브젝트를 활성화한다.
            buttons.gameObject.SetActive(true);
            //'게임 오버'상태로 변경
            gState = GameState.GameOver;
            //마우스 활성화
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        //만일, 플레이어의 hp가 0 이하라면
        if (player.chaHp <= 0f)
        {
            //HP바 처리
            player.hpSlider.value = 0;
            //플레이어의 애니메이션을 멈춘다.
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            //상태 텍스트를 활성화한다.
            gameLabel.SetActive(true);
            //상태 텍스트내용'Game Over'로 변경
            gameText.text = "Game Over";
            // 상태 텍스트의 색상을 붉은색으로 한다.
            gameText.color = new Color32(255, 0, 0, 255);
            //상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져온다.
            Transform buttons = gameText.transform.GetChild(0);
            //버튼 오브젝트를 활성화한다.
            buttons.gameObject.SetActive(true);
            //'게임 오버'상태로 변경
            gState = GameState.GameOver;
            //마우스 활성화
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

        //만약 esc키 입력시 옵션창 오픈
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openOptionWindow();
        }
    }
}
