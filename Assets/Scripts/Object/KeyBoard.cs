using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    public List<Button> buttons; // 버튼 목록
    public Button enterButton; // 엔터 버튼
    public Button backspaceButton; // 백스페이스 버튼

    [SerializeField]
    GameObject keyBoard;

    [SerializeField]
    ControlUIManager controlUIMgr;

    void Start()
    {
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

        enterButton.onClick.AddListener(CheckAnswer);
        backspaceButton.onClick.AddListener(RemoveCharacter);
    }

    public void AddCharacter(string character)
    {   
        controlUIMgr.AddCharacter(character);
    }

    public void CheckAnswer()
    {   
        controlUIMgr.CheckAnswer();
    }

    public void RemoveCharacter()
    {
        controlUIMgr.RemoveCharacter();
    }

    public void OnButtonClick(string character)
    {
        Debug.Log("버튼 클릭됨, 문자: " + character);
        AddCharacter(character);
    }
}
