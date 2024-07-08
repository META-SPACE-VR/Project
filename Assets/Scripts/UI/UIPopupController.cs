using UnityEngine;

public class UIPopupController : MonoBehaviour
{
    public GameObject popupPanel; // 팝업으로 사용할 Panel
    public GameObject toggleButton; // 팝업을 나타나게 하는 버튼

    public void TogglePopup()
    {
        if (popupPanel != null && toggleButton != null)
        {
            bool isActive = popupPanel.activeSelf;
            popupPanel.SetActive(!isActive); // 현재 상태의 반대로 설정하여 활성/비활성화
            toggleButton.SetActive(isActive); // 팝업이 나타나면 버튼을 숨기고, 팝업이 사라지면 버튼을 나타냄
        }
    }
}
