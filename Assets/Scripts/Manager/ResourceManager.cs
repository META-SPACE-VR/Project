using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using FusionExamples.Utility;


public class ResourceManager : MonoBehaviour
{
    public Color[] playerColours = new Color[12]; //플레이어 색상 저장
	public WorldCanvasNickname worldCanvasNicknamePrefab; //플레이어 닉네임을 표시하는 캔버스
	readonly List<NetworkObject> managedObjects = new List<NetworkObject>(); // 관리 중인 네트워크 오브젝트 목록

	public static ResourceManager Instance => Singleton<ResourceManager>.Instance;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

    // 네트워크 오브젝트를 관리 목록에 추가하는 함수
    public void Manage(NetworkObject obj)
    {
        managedObjects.Add(obj);
    }

    // 관리 목록에 있는 모든 오브젝트를 제거하는 함수
    public void Purge()
    {
        // 관리 목록에 있는 각 오브젝트를 순회하며 제거
        foreach (var obj in managedObjects)
        {
            if (obj) 
                GameManager.Instance.Runner.Despawn(obj);
        }
        // 관리 목록을 비움
        managedObjects.Clear();
    }
}
