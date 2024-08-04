using UnityEngine;
using TMPro;

public class TimerTMP : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TMP Text 요소를 할당받기 위한 변수
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

        // 시간을 시, 분, 초로 변환
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // UI 텍스트 업데이트
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
