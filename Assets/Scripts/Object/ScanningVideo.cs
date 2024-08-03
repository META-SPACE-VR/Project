using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScanningVideo : MonoBehaviour
{
    [SerializeField]
    VideoPlayer vp;

    [SerializeField]
    FaceScanUI ui;

    [SerializeField]
    AudioSource scanningSound;

    [SerializeField]
    int scanningFrame;

    [SerializeField]
    AudioSource scanSuccessSound;

    [SerializeField]
    int scanSuccessFrame;

    // Start is called before the first frame update
    void Start()
    {
        // 영상이 종료될 때 VideoFinished 함수가 동작하도록 설정
        vp.loopPointReached += VideoFinished;
    }

    private void Update() {
        if(vp.frame == scanningFrame) {
            scanningSound.Play();
        }

        if(vp.frame == scanSuccessFrame) {
            scanSuccessSound.Play();
        }
    }

    // 영상 종료 시 발생하는 함수
    void VideoFinished(VideoPlayer vp) {
        ui.VerifySuccess();
        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
