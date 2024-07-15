using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TMP_InputField textInput1;  // Quiz 1 답 입력 칸
    public TMP_InputField textInput2;  // Quiz 2 답 입력 칸
    public GameObject quiz1UI;
    public GameObject quiz2UI;
    public GameObject keyboard;        // 키보드 UI
    public KeyBoardOn keyBoardOn;      // 키보드 스크립트 참조

    public string answer1 = "25";
    public string answer2 = "1200";

    public GameObject mainUI;

    public void ShowMain()
    {
        quiz1UI.SetActive(false);
        quiz2UI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void ShowQuiz1()
    {
        mainUI.SetActive(true);
        quiz1UI.SetActive(true);
        quiz2UI.SetActive(false);
        textInput1.gameObject.SetActive(true);
        textInput2.gameObject.SetActive(false);
        keyBoardOn.ActivateKeyboard(textInput1, textInput1.textComponent, "num1ans");
    }

    public void ShowQuiz2()
    {
        mainUI.SetActive(true);
        quiz1UI.SetActive(false);
        quiz2UI.SetActive(true);
        textInput1.gameObject.SetActive(false);
        textInput2.gameObject.SetActive(true);
        keyBoardOn.ActivateKeyboard(textInput2, textInput2.textComponent, "num2ans");
    }

    public void ShowKeyboard()
    {
        keyboard.SetActive(true);
    }

    public void HideKeyboard()
    {
        keyboard.SetActive(false);
    }
}
