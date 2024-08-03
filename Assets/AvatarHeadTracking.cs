using UnityEngine;

public class AvatarHeadTracking : MonoBehaviour
{
    public OVRCameraRig cameraRig;

    [SerializeField]
    private Transform headTransform;
    void Start()
    {
        // 아바타의 머리 본 찾기 (본 이름이 "Head"라고 가정)
        // headBone = avatar.transform.Find("Armature/Head");

        // if (headBone == null)
        // {
        //     Debug.LogError("Head bone not found in the avatar.");
        //     return;
        // }

        // OVRCameraRig의 Tracking Space와 Center Eye Anchor 설정
        cameraRig.trackingSpace.transform.SetParent(headTransform);
        cameraRig.trackingSpace.transform.localPosition = Vector3.zero;
        cameraRig.trackingSpace.transform.localRotation = Quaternion.identity;

        cameraRig.centerEyeAnchor.transform.localPosition = Vector3.zero;
        cameraRig.centerEyeAnchor.transform.localRotation = Quaternion.identity;
    }
}
