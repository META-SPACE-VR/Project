using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardOn : MonoBehaviour
{   
    public TMP_Text textInput; // 텍스트 입력 표시를 위한 TMP_Text
    public string correctPassword; // 올바른 비밀번호 설정
    public GameObject newScreen; // 올바른 비밀번호일 때 표시할 UI 요소
    public GameObject wrongPanel; // 비밀번호가 틀릴 때 표시할 패널
    public List<Button> buttons; // 버튼 목록
    public Button enterButton; // 엔터 버튼
    public Button backspaceButton; // 백스페이스 버튼
    public Canvas[] otherCanvases; // The other canvases you want to hide

    [SerializeField]
    GameObject keyBoard;
    private string currentInput = "";

    // Start is called before the first frame update
    void Start()
    {
        wrongPanel.SetActive(false); // 처음에는 잘못된 비밀번호 패널을 숨김
        newScreen.SetActive(false); // 처음에는 새로운 화면 숨김


        UpdateDisplay(); // text를 textInputField에 계속 업데이트 함

        foreach (Button button in buttons)
        {
            if (button != null)
            {
                string character = button.name; // Use button's name instead of text
                button.onClick.AddListener(() => OnButtonClick(character));
                Debug.Log("버튼 할당됨: " + button.name);
            }
            else
            {
                Debug.LogError("버튼 목록에 null 값이 있습니다.");
            }
        }
        foreach (Canvas canvas in otherCanvases)
        {
            canvas.enabled = false;
        }

        enterButton.onClick.AddListener(CheckPassword);
        backspaceButton.onClick.AddListener(RemoveCharacter);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 현재 입력에 문자를 추가하는 메서드
    public void AddCharacter(string character)
    {
        Debug.Log("문자 추가됨: " + character);
        if (currentInput.Length < correctPassword.Length)
        {
            currentInput += character;
            UpdateDisplay();
            wrongPanel.SetActive(false); // 새로운 문자가 추가될 때마다 잘못된 비밀번호 패널을 숨김
        }
    }

    private void UpdateDisplay()
    {
        textInput.text = currentInput;
    }

    public void CheckPassword()
    {
        Debug.Log("비밀번호 확인됨, 현재 입력: " + currentInput);
        if (currentInput == correctPassword)
        {
            Debug.Log("비밀번호 일치!");
            SwitchToNewScreen();
        }
        else
        {
            Debug.Log("비밀번호 불일치!");
            wrongPanel.SetActive(true); // 잘못된 비밀번호 패널을 표시
        }
    }

    // 현재 입력에서 마지막 문자를 제거하는 메서드
    public void RemoveCharacter()
    {
        Debug.Log("마지막 문자 제거됨");
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
            wrongPanel.SetActive(false); // 문자가 제거될 때마다 잘못된 비밀번호 패널을 숨김
        }
    }

    private void SwitchToNewScreen()
    {
        newScreen.SetActive(true);
        wrongPanel.SetActive(false); // 새로운 화면으로 전환할 때 잘못된 비밀번호 패널을 숨김

        // 로그인 화면 비활성화
        textInput.transform.parent.gameObject.SetActive(false);

    }


    // 버튼 클릭을 처리하는 메서드
    public void OnButtonClick(string character)
    {
        Debug.Log("버튼 클릭됨, 문자: " + character);
        AddCharacter(character);
    }
}
