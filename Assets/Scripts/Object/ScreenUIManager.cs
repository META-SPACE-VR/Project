using UnityEngine;
using UnityEngine.UI;

public class ScreenUIManager : MonoBehaviour
{
    public GameObject screenCanvas; // Canvas 오브젝트 참조
    public InputField passwordInputField; // 비밀번호 입력 필드 참조
    public Button submitButton; // 제출 버튼 참조

    void Start()
    {
        screenCanvas.SetActive(false); // 처음에는 비활성화
        submitButton.onClick.AddListener(OnSubmitButtonClicked); // 버튼 클릭 이벤트 추가
    }

    public void ShowScreenUI()
    {
        screenCanvas.SetActive(true); // Canvas 활성화
    }

    public void HideScreenUI()
    {
        screenCanvas.SetActive(false); // Canvas 비활성화
    }

    void OnSubmitButtonClicked()
    {
        string password = passwordInputField.text;
        Debug.Log("Password entered: " + password);
        // 비밀번호 확인 로직 추가
    }
}
