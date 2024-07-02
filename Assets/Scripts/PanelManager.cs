using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HolePair {
    public GameObject cableHole1;
    public GameObject cableHole2;

    public HolePair(GameObject _cableHole1, GameObject _cableHole2) {
        cableHole1 = _cableHole1;
        cableHole2 = _cableHole2;
    }
}

public class PanelManager : MonoBehaviour
{
    // 올바른 케이블 연결
    [SerializeField]
    List<HolePair> answer;

    Dictionary<GameObject, GameObject> answerDict = new Dictionary<GameObject, GameObject>();

    // 사용하는 케이블들 (플레이어가 직접 손대는)
    [SerializeField]
    List<Cable> cables;

    // 동작까지 남은 연결 수
    public int remain;

    void Start() {
        remain = answer.Count;

        foreach(HolePair holePair in answer) {
            answerDict.Add(holePair.cableHole1, holePair.cableHole2);
            answerDict.Add(holePair.cableHole2, holePair.cableHole1);
        }
    }

    // 올바른 연결 쌍인지 확인
    bool CheckCorrectPair(GameObject cableHole1, GameObject cableHole2) {
        return answerDict.TryGetValue(cableHole1, out GameObject target) && target == cableHole2;
    }

    // 두 구멍이 연결됨
    public void ConnectPair(GameObject cableHole1, GameObject cableHole2) {
        if(cableHole1 && cableHole2 && CheckCorrectPair(cableHole1, cableHole2)) {
            remain--;
        }
    }

    // 두 구멍이 연결 해제 됨
    public void DisconnectPair(GameObject cableHole1, GameObject cableHole2) {
        if(cableHole1 && cableHole2 && CheckCorrectPair(cableHole1, cableHole2)) {
            remain++;
        }
    }
}
