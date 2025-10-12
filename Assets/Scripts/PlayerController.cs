using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed = 5.0f;
    public bool isMoving = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // 디버그: 제대로 찾았는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }
    }

    void Update()
    {
        // 이동 벡터 계산
        Vector3 movement = Vector3.zero;
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
            isMoving = true;
        }
    
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1); // 원래 크기
            isMoving = true;
        }
        
        // 실제 이동 적용
        if (movement != Vector3.zero)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        
        // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
        float currentSpeed = movement != Vector3.zero ? moveSpeed : 0f;
        
        // Animator에 속도 전달
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            Debug.Log("Current Speed: " + currentSpeed);
        }
    }
}
