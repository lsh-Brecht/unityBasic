using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>(); //스크립트도 가져올 수 있음
        hands = GetComponentsInChildren<Hand>(true); //true 값을 주면 inactive된 오브젝트로 가져옴
    }

    private void OnEnable() {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Start()
    {
        
    }

    ////구버전
    //void Update()
    //{
    //    //GetAxis(부드러운 보정, x와 y가 0에서 1까지 커짐)
    //    //GetAxisRaw(입력에 딱 맞음, x와 y가 0 아니면 1)
    //    inputVec.x = Input.GetAxis("Horizontal");
    //    inputVec.y = Input.GetAxis("Vertical");
    //}

    //1. 물리 기반 이동, 힘을 가해서 이동하니까 점점 가속하므로 느리게 출발해 멈출 때는 밀리는 느낌
    //mass, drag 등의 물리 속성에 영향 받음.
    //rigid.AddForce(inputVec);
    //2. 즉각적인 속도. 가속 없이 바로 원하는 속도로 움직임.
    //rigid.linearVelocity = inputVec;

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // normalize로 대각선 움직임도 1로 맞춰줌.
        //deltaTime은 Update에서 fixedDeltaTime은 FixedUpdate에서 사용.
        
        //3. 위치 기반 이동, 내부적으로 물리 계산을 통해 움직임. 반응성은 위와 비교했을 때 중간.
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value) {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate() {
        if (!GameManager.instance.isLive) return;
        animator.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            spriteR.flipX = (inputVec.x < 0) ? true : false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime * 10;
        // int형에서 float을 빼는 문제(health를 float으로 수정)
        if (GameManager.instance.health <= 0) {
            for (int i = 2; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
