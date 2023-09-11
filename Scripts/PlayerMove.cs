using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //이동속도 변수
    public float moveSpeed = 5f;
    //캐릭터 컨트롤러 변수선언
    CharacterController cc;
    //중력속도 변수
    float gravity = -10f;
    //수직속력 변수
    float yVelocity = 0;
    //점프력 변수
    public float jumpPower = 2f;
    //점프상태 변수
    bool isJumping = false;
    //캐릭터의 체력
    public int chaHp = 20;
    //최대 체력 변수
    int maxHp = 20;
    //hp 슬라이더 변수
    public Slider hpSlider;

    //Hit 효과 오브젝트
    public GameObject hitEffect;

    //애니메이터 변수
    Animator anim;



    //플레이어의 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미의 공격력 만큼 플레이어의 체력을 깎는다.
        chaHp -= damage;

        //만일 , 플레이어의 체력이 0보다 크면 피격 효과를 출력한다.
        if(chaHp > 0)
        {
            //피격 코루틴을 시작한다
            StartCoroutine(PlayHitEffect());
        }
    }
    
    //피격 효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        //1. 피격 UI를 활성화한다.
        hitEffect.SetActive(true);
        //2. 0.3초간 대기한다.
        yield return new WaitForSeconds(0.3f);

        //3.피격 UI를 비활성화한다.
        hitEffect.SetActive(false);
    }
    
    private void Start()
    {
        // 캐릭터 컨트롤러 변수 선언
         cc = GetComponent<CharacterController>();
        
        //애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //스페이스바 입력을 받으면 캐릭터를 그 방향으로 이동시키고 싶다.
        //만일,다시 바닥에 착지했다면
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //만일, 점프 중이었다면..
            if (isJumping)
            {
                //점프 전 상태로 초기화한다.
                isJumping = false;
            }
            //캐릭터 수직 속도를 0으로 만든다.
            yVelocity = 0;
        }
        //space바 입력을 받고 점프 상태가 아니라면
        if (Input.GetButton("Jump") && !isJumping)
        {
            //캐릭터 수직 속도에 점프력을 적용한다. 점프상태로 변경
            yVelocity = jumpPower;
            isJumping = true;
        }

        //w,a,s,d 입력을 받으면 캐릭터를 그 방향으로 이동시키고 싶다.
        //1,사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        //2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3 (h, 0, v);
        dir = dir.normalized;

        //이동 블랜딩 트리를 호출하고 백터의 크기 값을 넘겨준다.
        anim.SetFloat("MoveMotion", dir.magnitude);

        //2-1. 메인카메라 회전 방향에 따라 벡터 변경
        dir = Camera.main.transform.TransformDirection(dir);

        //2-2.캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        //3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);


        //4. 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영한다.
        hpSlider.value = (float)chaHp / (float)maxHp;
        
    }
}
