using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurgeryController : MonoBehaviour
{
    public Button SuctionBtn;
    public Button BleedStopBtn;
    public Button CutBtn; // 새 버튼 추가
    public Animator machineAnimator; // Animator 컴포넌트를 참조합니다.
    public Camera surgeryCamera; // 카메라 참조

    void Start()
    {
        // 버튼 클릭 이벤트 설정
        SuctionBtn.onClick.AddListener(OnSuctionButtonClicked);
        BleedStopBtn.onClick.AddListener(OnBleedStopButtonClicked);
        CutBtn.onClick.AddListener(OnCutButtonClicked); // 새 버튼 클릭 이벤트 설정
    }

    void OnSuctionButtonClicked()
    {
        // Center_Suck 트리거 활성화
        Debug.Log("뭐임");
        machineAnimator.SetTrigger("Center_Suck");
    }

    void OnBleedStopButtonClicked()
    {
        // Right_BleedStop 트리거 활성화
        machineAnimator.SetTrigger("Right_BleedStop");
    }

    void OnCutButtonClicked()
    {
        // 카메라를 왼쪽으로 -0.02만큼 이동
        Vector3 newPosition = surgeryCamera.transform.position;
        newPosition.x += 0.02f;
        surgeryCamera.transform.position = newPosition;
    }
}
