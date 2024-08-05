using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵에 대한 데이터를 포함합니다. 여기에는 주 게임 맵과 프리게임 맵 모두가 포함되며, 스폰 포인트를 참조하는 데 사용됩니다.
/// </summary>
public class MapData : MonoBehaviour
{
    // 맵의 벽 경계를 나타내는 콜라이더에 대한 참조입니다.
    public Collider wallCollider;

    // 맵의 헐(예: 시각적 경계 또는 구조물)을 나타내는 게임 오브젝트입니다.
    public GameObject hull;

    // 맵에서 스폰 포인트를 나타내는 트랜스폼 배열입니다.
    public Transform[] spawns;

    /// <summary>
    /// 제공된 상태에 따라 벽 콜라이더를 활성화하거나 비활성화합니다.
    /// </summary>
    /// <param name="state">콜라이더를 활성화하려면 true, 비활성화하려면 false입니다.</param>
    public void SetWallColliders(bool state)
    {
        wallCollider.enabled = state;
    }

    /// <summary>
    /// 주어진 인덱스에 따라 스폰 포인트의 위치를 가져옵니다.
    /// </summary>
    /// <param name="index">스폰 포인트 배열에서의 인덱스입니다.</param>
    /// <returns>지정된 스폰 포인트의 위치입니다.</returns>
    public Vector3 GetSpawnPosition(int index)
    {
        // 인덱스가 배열의 범위 내에 있는지 확인하여 런타임 오류를 방지합니다.
        if (index >= 0 && index < spawns.Length)
        {
            return spawns[index].position;
        }
        else
        {
            // 옵션으로 예외를 던지거나 기본값을 반환할 수 있습니다.
            Debug.LogError("스폰 인덱스가 범위를 초과했습니다.");
            return Vector3.zero;
        }
    }
}