using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed; // 걷기 속도
    [SerializeField]
    private float runSpeed; // 뛰기 속도
    [SerializeField]
    private float sitSpeed; // 앉기 속도

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isSit = false;
    private bool isGround = true;
    private bool isWalkBackwards = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을때, 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float sitPosY;
    private float originPosY;
    private float applySitPosY;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField]
    private float lookSensitivity; // 회전 속도 더 낮춤

    // 카메라 
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    // 애니메이터 컴포넌트
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applySitPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TrySit();
        Move();
        CharacterRotation();
        CameraRotation();
        UpdateAnimator();
    }

    private void TrySit()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Sit();
        }
    }

    private void Sit()
    {
        isSit = !isSit;

        if (isSit)
        {
            applySpeed = sitSpeed;
            applySitPosY = sitPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applySitPosY = originPosY;
        }
        StartCoroutine(SitCoroutine());
    }

    IEnumerator SitCoroutine()
    {
        float startY = theCamera.transform.localPosition.y;
        float elapsedTime = 0f;
        float duration = 0.3f; // 보간에 사용할 시간

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startY, applySitPosY, elapsedTime / duration);
            theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, newY, theCamera.transform.localPosition.z);
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applySitPosY, theCamera.transform.localPosition.z);
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isSit)
            Sit();
        myRigid.velocity = transform.up * jumpForce;
        animator.SetTrigger("Jump");
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isSit)
            Sit();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        // 앞 방향 이동 우선
        if (moveDirZ > 0)
        {
            moveDirX *= 0.5f; // 좌우 이동의 비율을 낮춰서 앞으로 가는 게 더 우선하도록 함
        }

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 moveDir = (moveHorizontal + moveVertical).normalized;

        Vector3 velocity = moveDir * applySpeed;
        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);

        // 이동 중인지 확인
        isWalk = (moveDirX != 0 || moveDirZ > 0);
        isWalkBackwards = (moveDirZ < 0);

        if (isRun)
        {
            isWalk = false;
        }

        if (isWalk || isRun)
        {
            // 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.01f); // 회전 속도를 조정
        }
    }



    private void UpdateAnimator()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Run", isRun);
        animator.SetBool("isWalkBackwards", isWalkBackwards); // 뒤로 걷기 애니메이션 업데이트
    }

    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}