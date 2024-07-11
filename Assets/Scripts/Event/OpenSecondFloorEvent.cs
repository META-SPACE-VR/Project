using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenSecondFloorEvent : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera; // 메인 카메라

    [SerializeField]
    Camera targetCamera; // 목표 카메라

    [SerializeField]
    FadePanel fadePanel; // 페이드 판넬

    [SerializeField]
    List<GameObject> targetObjects; // 레이저의 목표 오브젝트

    [SerializeField]
    GameObject laserPointer; // 레이저 포인터

    [SerializeField]
    float laserMoveLength; // 레이저 이동 길이

    Vector3 onPos;
    Vector3 offPos;
    Quaternion initRot;

    [SerializeField]
    float laserMoveDuration; // 레이저 이동 소요 시간

    bool isPlaying = false; // 이벤트 재생 상태

    float currentTime = 0f;

    void Start() {
        onPos = laserPointer.transform.position;
        offPos = laserPointer.transform.position + Vector3.up * laserMoveLength;
        initRot = laserPointer.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && !isPlaying) {
            StartCoroutine(PlayEvent());
        }
    }

    IEnumerator PlayEvent() {
        isPlaying = true;

        // Scene Fade Out
        yield return fadePanel.FadeOut();

        // 카메라 전환
        mainCamera.enabled = false;
        targetCamera.enabled = true;

        // Scene Fade In
        yield return fadePanel.FadeIn();

        // 레이져 등장
        laserPointer.transform.position = new(laserPointer.transform.position.x, offPos.y, laserPointer.transform.position.z);
        laserPointer.SetActive(true);

        while(laserPointer.transform.position.y > onPos.y) {
            currentTime += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(offPos, onPos, currentTime / laserMoveDuration);
            if(newPos.y < onPos.y) {
                laserPointer.transform.position = new(newPos.x, onPos.y, newPos.z);
            }
            else {
                laserPointer.transform.position = newPos;
            }
            
            yield return null;
        }
        currentTime = 0f;

        yield return new WaitForSeconds(1f);

        // 타겟을 향해 조준 -> 발사
        foreach(GameObject targetObj in targetObjects) {
            if(!targetObj.activeSelf || targetObj.IsDestroyed()) continue;

            // 조준
            Quaternion targetRot = Quaternion.LookRotation(targetObj.transform.position - laserPointer.transform.position);
            Quaternion beforeRot = laserPointer.transform.rotation;

            while(true) {
                currentTime += Time.deltaTime;

                if(currentTime >= laserMoveDuration) {
                    laserPointer.transform.rotation = targetRot;
                    break;
                }
                else {
                    Quaternion newRot = Quaternion.Lerp(beforeRot, targetRot, currentTime / laserMoveDuration);
                    laserPointer.transform.rotation = newRot;
                }
                
                yield return null;
            }
            currentTime = 0f;
            
            yield return new WaitForSeconds(1f);
            
            // 발사
            laserPointer.GetComponent<LaserPointer>().Shoot();

            yield return new WaitForSeconds(1f);
        }

        // 원래 회전 상태로 복귀
        Quaternion befRot = laserPointer.transform.rotation;
        while(true) {
            currentTime += Time.deltaTime;

            if(currentTime >= laserMoveDuration) {
                laserPointer.transform.rotation = initRot;
                break;
            }
            else {
                Quaternion newRot = Quaternion.Lerp(befRot, initRot, currentTime / laserMoveDuration);
                laserPointer.transform.rotation = newRot;
            }
            
            yield return null;
        }
        currentTime = 0f;

        // 레이져 퇴장
        while(laserPointer.transform.position.y < offPos.y) {
            currentTime += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(onPos, offPos, currentTime / laserMoveDuration);
            if(newPos.y > offPos.y) {
                laserPointer.transform.position = new(newPos.x, offPos.y, newPos.z);
            }
            else {
                laserPointer.transform.position = newPos;
            }
            
            yield return null;
        }
        currentTime = 0f;

        laserPointer.SetActive(false);

        yield return new WaitForSeconds(1f);

        // Scene Fade Out
        yield return fadePanel.FadeOut();

        // 카메라 전환
        mainCamera.enabled = true;
        targetCamera.enabled = false;

        // Scene Fade In
        yield return fadePanel.FadeIn();

        isPlaying = false;
    }
}
