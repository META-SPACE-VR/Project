using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f; // 경과 시간

    // 게임이 시작될 때 호출되는 메서드
    void Start()
    {
        elapsedTime = 0f;
    }

    // 매 프레임마다 호출되는 메서드
    void Update()
    {
        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;
    }

    public string GetTimeString() {
        // 시간을 시, 분, 초로 변환
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int miliSeconds = Mathf.FloorToInt((elapsedTime % 1f) * 1000f);

        // 시간 형식으로 문자열 생성 후 리턴
        return string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, miliSeconds);
    }
}
