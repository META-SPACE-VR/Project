using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Goods : MonoBehaviour
{

    bool hasFood = true;

    void Start() {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(GiveItem);
    }

    private void GiveItem(SelectEnterEventArgs args) {
        // 아이템을 가지고 있지 않으면 그대로 종료
        if(!hasFood) return;

        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        
        if (interactor != null)
        {   
            // 테스트 로그
            Debug.Log("Get Food!!!!!");

            /* 상호작용한 플레이어에게 식량 아이템 지급하는 로직
            *
            */

            // 아이템을 지급했으므로 hasFood를 false로 전환
            hasFood = false;
        }
    }
}
