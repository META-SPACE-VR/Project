using UnityEngine;

public class OVRPlayerControllerWithAnimation : MonoBehaviour
{
    public OVRPlayerController ovrPlayerController;
    public Animator animator;
    public Transform avatarTransform;

    private void Update()
    {
        // 플레이어의 속도를 가져옴
        Vector3 velocity = ovrPlayerController.GetComponent<CharacterController>().velocity;

        // 이동 중인지 확인
        bool isWalking = velocity.magnitude > 0.1f;

        // 애니메이터의 "Walk" 파라미터를 설정
        animator.SetBool("Walk", isWalking);

        // 플레이어의 회전 값을 가져와서 아바타에 적용
        Quaternion playerRotation = ovrPlayerController.transform.rotation;
        avatarTransform.rotation = playerRotation;
    }
}
