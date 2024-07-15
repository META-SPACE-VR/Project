using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardOn : MonoBehaviour
{
    public string loginPassword = "me1234"; // 로그인 비밀번호
    public string quiz1Answer = "25"; // 퀴즈 1 답
    public string quiz2Answer = "1200"; // 퀴즈 2 답

    public GameObject loginScreen; // 로그인 화면 UI 요소
    public GameObject newScreen; // 올바른 비밀번호일 때 표시할 UI 요소
    public GameObject wrongPanel; // 비밀번호가 틀릴 때 표시할 패널
    public List<Button> buttons; // 버튼 목록
    public Button enterButton; // 엔터 버튼
    public Button backspaceButton; // 백스페이스 버튼
    public Canvas[] otherCanvases; // The other canvases you want to hide

    [SerializeField]
    GameObject keyBoard;
    private TMP_InputField activeInputField; // 현재 활성화된 InputField
    private TMP_Text activeTextInput; // 현재 활성화된 텍스트 인풋 필드의 텍스트를 표시할 TMP_Text
    private string currentButtonName; // 현재 버튼 이름

    void Start()
    {
        wrongPanel.SetActive(false); // 처음에는 잘못된 비밀번호 패널을 숨김
        newScreen.SetActive(false); // 처음에는 새로운 화면 숨김
        keyBoard.SetActive(false); // 처음에는 키보드 숨김

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

        enterButton.onClick.AddListener(CheckAnswer);
        backspaceButton.onClick.AddListener(RemoveCharacter);
    }

    void UpdateDisplay()
    {
        if (activeInputField != null && activeTextInput != null)
        {
            activeTextInput.text = activeInputField.text;
        }
    }

    public void AddCharacter(string character)
    {
        Debug.Log("문자 추가됨: " + character);
        if (activeInputField != null)
        {
            activeInputField.text += character;
            wrongPanel.SetActive(false); // 새로운 문자가 추가될 때마다 잘못된 비밀번호 패널을 숨김
            UpdateDisplay();
        }
    }

    public void CheckAnswer()
    {
        if (activeInputField == null)
            return;

        Debug.Log("답 확인됨, 현재 입력: " + activeInputField.text);
        string userInput = activeInputField.text;
        bool isCorrect = false;

        switch (currentButtonName)
        {
            case "InputFieldBtn":
                isCorrect = userInput == loginPassword;
                if (isCorrect)
                {
                    loginScreen.SetActive(false);
                    newScreen.SetActive(true);
                }
                break;
            case "num1ans":
                isCorrect = userInput == quiz1Answer;
                break;
            case "num2ans":
                isCorrect = userInput == quiz2Answer;
                break;
        }

        if (isCorrect)
        {
            Debug.Log("답 일치!");
            SwitchToNewScreen();
        }
        else
        {
            Debug.Log("답 불일치!");
            wrongPanel.SetActive(true); // 잘못된 답 패널을 표시
        }
    }

    public void RemoveCharacter()
    {
        if (activeInputField == null)
            return;

        Debug.Log("마지막 문자 제거됨");
        if (activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
            wrongPanel.SetActive(false); // 문자가 제거될 때마다 잘못된 답 패널을 숨김
            UpdateDisplay();
        }
    }

    private void SwitchToNewScreen()
    {
        newScreen.SetActive(true);
        wrongPanel.SetActive(false); // 새로운 화면으로 전환할 때 잘못된 답 패널을 숨김

        // 키보드와 텍스트 입력 필드 비활성화
        keyBoard.SetActive(false);
        if (activeInputField != null)
        {
            activeInputField.gameObject.SetActive(false);
        }
        if (activeTextInput != null)
        {
            activeTextInput.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick(string character)
    {
        Debug.Log("버튼 클릭됨, 문자: " + character);
        AddCharacter(character);
    }

    public void ActivateKeyboard(TMP_InputField inputField, TMP_Text textInput, string buttonName)
    {
        activeInputField = inputField;
        activeTextInput = textInput;
        currentButtonName = buttonName;
        keyBoard.SetActive(true);
        UpdateDisplay();
    }

    public void ActivateKeyboardFromButton(GameObject inputFieldButton)
    {
        TMP_InputField inputField = inputFieldButton.GetComponentInChildren<TMP_InputField>();
        TMP_Text textInput = inputFieldButton.GetComponentInChildren<TMP_Text>();
        if (inputField != null && textInput != null)
        {
            ActivateKeyboard(inputField, textInput, inputFieldButton.name);
        }
    }
}
