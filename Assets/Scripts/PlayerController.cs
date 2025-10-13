using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public bool isMoving = false;
    private bool isGrounded = false;

    private int score = 0;

    private Vector3 startPosition;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        // 디버그: 제대로 찾았는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다! Player에 추가하세요.");
        }

        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);
    }

    void Update()
    {
        // 이동 벡터 계산
        Vector3 movement = Vector3.zero;
        float moveX = 0f;
        
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
            isMoving = true;
        }
    
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            transform.localScale = new Vector3(1, 1, 1); // 원래 크기
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2f;
            Debug.Log("달리기 모드 활성화!");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2f;
            Debug.Log("달리기 모드 비활성화!");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (animator != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                Debug.Log("점프!");
            }
        }
        
        // 실제 이동 적용
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        
        // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        if (currentSpeed <= 0) {isMoving = false;}
        
        // Animator에 속도 전달
        if (animator != null)
        {
            animator.SetFloat("speed", currentSpeed);
            //Debug.Log("Current Speed: " + currentSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 시작: " + collision.gameObject.name);
        isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");
            
            // 시작 위치로 순간이동
            transform.position = startPosition;
            
            // 속도 초기화 (안 하면 계속 날아감)
            rb.linearVelocity = new Vector2(0,0);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("충돌 종료: " + collision.gameObject.name);
        isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gem"))
        {
            score++;  // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  // 코인 제거
        }
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
            
            // 캐릭터 조작 비활성화
            enabled = false;
        }
    }
}
