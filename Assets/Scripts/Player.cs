using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>(); //��ũ��Ʈ�� ������ �� ����
        hands = GetComponentsInChildren<Hand>(true); //true ���� �ָ� inactive�� ������Ʈ�� ������
    }

    void Start()
    {
        
    }

    ////������
    //void Update()
    //{
    //    //GetAxis(�ε巯�� ����, x�� y�� 0���� 1���� Ŀ��)
    //    //GetAxisRaw(�Է¿� �� ����, x�� y�� 0 �ƴϸ� 1)
    //    inputVec.x = Input.GetAxis("Horizontal");
    //    inputVec.y = Input.GetAxis("Vertical");
    //}

    //1. ���� ��� �̵�, ���� ���ؼ� �̵��ϴϱ� ���� �����ϹǷ� ������ ����� ���� ���� �и��� ����
    //mass, drag ���� ���� �Ӽ��� ���� ����.
    //rigid.AddForce(inputVec);
    //2. �ﰢ���� �ӵ�. ���� ���� �ٷ� ���ϴ� �ӵ��� ������.
    //rigid.linearVelocity = inputVec;

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // normalize�� �밢�� �����ӵ� 1�� ������.
        //deltaTime�� Update���� fixedDeltaTime�� FixedUpdate���� ���.
        
        //3. ��ġ ��� �̵�, ���������� ���� ����� ���� ������. �������� ���� ������ �� �߰�.
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
}
