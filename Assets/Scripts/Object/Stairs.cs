using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    Transform startTransform; // 처음 위치

    [SerializeField]
    Transform targetTransform; // 목표 위치

    [SerializeField]
    float movingTime; // 소요 시간

    [SerializeField]
    AudioSource moveSound;

    float currentTime = 0f; // 움직이는데 소요한 시간

    bool isUsed = false; // 사용 여부

    private void Awake() {
        // 계단을 처음 위치로 이동시킨다.
        gameObject.transform.position = startTransform.position;
    }

    private void Update() {
        if(isUsed && currentTime < movingTime) {
            if(currentTime == 0f) {
                moveSound.Play();
            }

            currentTime += Time.deltaTime;
            if(currentTime >= movingTime) currentTime = movingTime;

            Vector3 nextPosition = Vector3.Lerp(startTransform.position, targetTransform.position, currentTime / movingTime);
            gameObject.transform.position = nextPosition;

            if(currentTime == movingTime) {
                moveSound.Stop();
            }
        }
    }

    // 계단 사용
    public void Use() {
        isUsed = true;
    }

    public float GetMovingTime() {
        return movingTime;
    }

    public bool isFinished() {
        return isUsed && (currentTime == movingTime);
    }
}
