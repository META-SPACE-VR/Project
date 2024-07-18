using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    List<TMP_InputField> inputFields; // 각 UI에서 사용하는 InputField들
    [SerializeField]
    List<string> answers; // 정답
    [SerializeField]
    GameObject keyboard; // 키보드

    TMP_InputField activeInputField; // 활성화된 InputField

    public bool isMouseEntered; // 마우스 커서가 내부에 있으면 true

    private void Awake() {
        activeInputField = null;

        // inputField에 이벤트 추가
        foreach (TMP_InputField inputField in inputFields) {
            inputField.onSelect.AddListener((_) => {
                activeInputField = inputField;
                keyboard.SetActive(true);
            });

            inputField.onDeselect.AddListener((_) => {
                if(isMouseEntered) {
                    if(activeInputField == inputField) {
                        activeInputField = null;
                        keyboard.SetActive(false);
                    }
                }
                else {
                    if(activeInputField == inputField) {
                        StartCoroutine(SelectActiveInputField());
                    }
                }
            });
        }
    }

    IEnumerator SelectActiveInputField() {
        yield return null;
        activeInputField.ActivateInputField();
        activeInputField.Select();
    }

    private void OnEnable() {
        // 오브젝트가 비활성화되다가 다시 활성화되면 모든 InputField의 값을 빈 문자열로 설정
        foreach (TMP_InputField inputField in inputFields) {
            inputField.text = "";
        }

        // 키보드 비활성화
        keyboard.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) { isMouseEntered = true; }
    public void OnPointerExit(PointerEventData eventData) { isMouseEntered = false; }

    // inputField에 문자 추가
    public void AddCharacter(string character) {
        Debug.Log("입력 : " + character);
        activeInputField.text += character;
    }

    // 맨 뒤 문자 제거
    public void RemoveCharacter()
    {
        if (activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text[..^1];
        }
    }

    // 답 확인
    public void CheckAnswer()
    {
        bool isCorrect = true;

        for (int i = 0; i < Mathf.Min(inputFields.Count, answers.Count) && isCorrect; i++) {
            if(inputFields[i].text != answers[i]) {
                isCorrect = false;
            }
        }

        if (isCorrect) // 비밀번호가 일치하면
        {   
            DoCorrectAction();
        }
        else { // 틀리면
            DoWrongAction();
        }
    }

    protected virtual void DoCorrectAction() { }
    protected virtual void DoWrongAction() { }
}
