using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    //생화학자, 기계공, 전기공, 의사 
    public string[] jobList = {"Biochemist","Mechanic","Electrician", "Doctor"};
    private List<string> availableJobs; //남아 있는 직업을 담을 리스트

    void Start()
    {
        ////직업 리스트를 복사하여 초기화
        availableJobs = new List<string>(jobList); 

        //게임 시작 시 각 플레이어에게 직업 할당
        AssignJobs(); 
    }

    //플레이어에게 직업을 할당하는 함수 
    void AssignJobs() 
    {
        // 플레이어를 나타내는 객체를 가져오는 코드
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            string randomJob = GetRandomJob();

            //랜덤으로 선택된 직업을 플레이어에게 할당 
            // PlayerJobController controller = player.GetComponent<PlayerJobController>();
            // if (controller!=null) PlayerJobController.SetJob(randomJob);
        }
    }

    string GetRandomJob()
    {
        if (availableJobs.Count==0) return ""; 

        int randomIndex = Random.Range(0, availableJobs.Count);
        string randomJob = availableJobs[randomIndex];
        Debug.Log("할당된 직업: " + randomJob);

        //할당된 직업을 리스트에서 제거 (중복 할당 방지)
        availableJobs.RemoveAt(randomIndex);
        
        return randomJob; 
    }
}
