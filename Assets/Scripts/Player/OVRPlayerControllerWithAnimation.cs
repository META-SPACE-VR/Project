using UnityEngine;

public class OVRPlayerControllerWithAnimation : MonoBehaviour
{
    public OVRPlayerController ovrPlayerController;  // OVRPlayerController 참조
    public Animator animator;  // Animator 참조
    public Transform avatarTransform;  // 아바타의 Transform 참조

    private void Update()
    {
        // 플레이어의 CharacterController에서 속도 벡터를 가져옵니다.
        Vector3 velocity = ovrPlayerController.GetComponent<CharacterController>().velocity;

        // 이동 중인지 확인
        bool isWalking = velocity.magnitude > 0.1f;

        // 달리기 버튼이 눌렸는지 확인
        bool isRunning = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || 
                         Input.GetKey(KeyCode.LeftShift) || 
                         Input.GetKey(KeyCode.RightShift);

        // 점프 버튼이 눌렸는지 확인
        bool isJumping = OVRInput.GetDown(OVRInput.Button.Three) ||
                        Input.GetKey(KeyCode.Space); 

        // 애니메이터의 "Walk", "Run", 및 "Jump" 파라미터를 설정
        animator.SetBool("Walk", isWalking && !isRunning);
        animator.SetBool("Run", isWalking && isRunning);

        if (isJumping)
        {
            animator.SetTrigger("Jump"); // 점프 애니메이션 트리거
            ovrPlayerController.Jump(); // 점프 수행
        }

        // 플레이어의 회전 값을 가져와서 아바타에 적용
        Quaternion playerRotation = ovrPlayerController.transform.rotation;
        avatarTransform.rotation = playerRotation;
    }
}
