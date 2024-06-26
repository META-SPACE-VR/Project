using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
    [SerializeField]
    Vector2 initPosInPuzzle; // 로봇 팔의 초기 위치 (퍼즐판 기준)

    Vector2 curPosInPuzzle; // 로봇 팔의 현재 위치 (퍼즐판 기준)
    Vector2 targetPosInPuzzle; // 로봇팔의 목표 위치 (퍼즐판 기준)
    Vector3 offset1 = Vector3.zero; // 보정 값 1
    Vector3 offset2 = Vector3.zero; // 보정 값 2

    [SerializeField]
    float unitLength; // 한칸의 단위 길이

    [SerializeField]
    float moveTime; // 한칸 움직이는데 걸리는 시간

    float movePercentage; // 움직인 비율

    bool isMoveReset = false; // 이동 초기화 여부
    GameObject selectedContainer = null; // 선택된 컨테이너

    [SerializeField]
    Transform attachPoint; // 부착 위치

    [SerializeField]
    Transform lightPoint; // 빛 기준점

    // Start is called before the first frame update
    void Start()
    {
        // 로봇 팔을 초기 위치에 둔다.
        curPosInPuzzle = targetPosInPuzzle = initPosInPuzzle;
        transform.position = new Vector3(unitLength * (initPosInPuzzle.x - 2.5f), transform.position.y, unitLength * (initPosInPuzzle.y - 2.5f));
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 이동중인 상태가 아니라면 이동 관련 입력을 확인
        if(movePercentage == 0) {
            CheckMoveInput();
        }

        Vector3 curPos = new Vector3(unitLength * (curPosInPuzzle.x - 2.5f), transform.position.y, unitLength * (curPosInPuzzle.y - 2.5f)) + offset1;
        Vector3 newPos = new Vector3(unitLength * (targetPosInPuzzle.x - 2.5f), transform.position.y, unitLength * (targetPosInPuzzle.y - 2.5f)) + offset2;

        // 현재 위치와 목표 위치가 다르면 새롭게 위치 설정
        if(curPos != newPos) {
            Move(curPos, newPos, isMoveReset); // 로봇팔의 위치 설정
        }
        else { // 같으면 컨테이너 부착 관련 입력 확인
            CheckAttachInput();
        }
    }

    // 이동 관련 입력을 확인해서 로봇 팔의 새 위치 계산
    void CheckMoveInput() {
        targetPosInPuzzle = curPosInPuzzle;

        if (Input.GetKeyDown(KeyCode.W)) {
            targetPosInPuzzle.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            targetPosInPuzzle.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            targetPosInPuzzle.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            targetPosInPuzzle.x += 1;
        }
    }

    // 현재 위치와 목표 위치에 따라 새로운 위치 계산 후 그 위치로 설정
    void Move(Vector3 curPos, Vector3 newPos, bool isBackWard) {
        // 다음 movePercentage 계산
        movePercentage += Time.deltaTime/moveTime * (isBackWard ? -1 : 1);
        movePercentage = Mathf.Min(1, movePercentage);
        movePercentage = Mathf.Max(0, movePercentage);
        
        // 새로운 위치 반영
        transform.position = Vector3.Lerp(curPos, newPos, movePercentage);

        // 이동이 종료되면 수행하는 동작
        if(movePercentage == 0 || movePercentage == 1) {
            if(movePercentage == 1) { // 이동이 제대로 종료되면
                curPosInPuzzle = targetPosInPuzzle;
                offset1 = offset2;
                
                if(selectedContainer) {
                    AttachContainer(selectedContainer);
                }
            }
            else { // 이동 초기화가 완료되면
                targetPosInPuzzle = curPosInPuzzle;
            }

            movePercentage = 0;
            isMoveReset = false;
        }
    }

    // 컨테이너 부착 관련 입력 확인
    void CheckAttachInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(selectedContainer) {
                DetachContainer();
                offset2 = Vector3.zero;
                selectedContainer = null;
            }
            else {
                if(Physics.Raycast(lightPoint.position, -1 * transform.up, out RaycastHit hit)) {
                    if(hit.collider.CompareTag("Container")) {
                        selectedContainer = hit.collider.gameObject;

                        Transform attachPoint = transform.GetChild(1);
                        offset2 = hit.collider.transform.position - attachPoint.position;
                        offset2.y = 0;

                        if(offset2 == Vector3.zero) {
                            AttachContainer(selectedContainer);
                        }
                    }
                }
            }
        }
    }

    // 로봇 팔에 컨테이너 부착
    void AttachContainer(GameObject container) {
        container.transform.SetParent(attachPoint);
        container.transform.localPosition = Vector3.zero;
        container.GetComponent<Rigidbody>().useGravity = false;
    }

    // 로봇 팔에서 컨테이너 제거 
    void DetachContainer() {
        GameObject container = attachPoint.GetChild(0).gameObject;
        
        attachPoint.DetachChildren();
        container.GetComponent<Rigidbody>().useGravity = true;
    }

    // 이동 초기화
    public void ResetMove() {
        isMoveReset = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Wall")) {
            ResetMove();
        }
    }
}
