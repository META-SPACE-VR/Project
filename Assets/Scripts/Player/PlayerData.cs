using Helpers.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 애니메이션 위치와 같은 네트워크와 관련이 없는 정보를 처리하는 컴포넌트
/// </summary>
public class PlayerData : MonoBehaviour
{
    
    RoomPlayer pObj; // 이 스크립트가 붙어 있는 플레이어의 정보를 담고 있는 객체
    public Transform uiPoint; // UI에서 플레이어의 이름을 표시할 위치
    WorldCanvasNickname nicknameUI; // 플레이어의 이름을 UI에 표시하는 컴포넌트

    [SerializeField] private string job;

    private void Awake()
    {
        // RoomPlayer 컴포넌트를 가져옴 
        pObj = GetComponent<RoomPlayer>(); 
    }

    // 플레이어의 이름을 설정하고 UI에 표시 
    public void SetNickname(string nickname)
    {
        // if (nicknameUI == null)
        // {
        //     // 닉네임 UI를 생성하고 위치를 설정
        //     nicknameUI = Instantiate(
        //         GameManager.rm.worldCanvasNicknamePrefab,
        //         uiPoint.transform.position,
        //         Quaternion.identity,
        //         GameManager.im.nicknameHolder);
        //     nicknameUI.target = uiPoint;
        // }
        // // 닉네임 UI에 이름을 설정
        // nicknameUI.worldNicknameText.text = nickname;
    }

    public void SetColour(Color col)
    {
        job = ColorUtility.ToHtmlStringRGB(col);
    }
}