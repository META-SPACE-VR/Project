using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    RobotArm robotArm;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        robotArm = GameObject.FindGameObjectWithTag("Robot Arm").GetComponent<RobotArm>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // 키네마틱 상태로 변경
    public void changeKinematic() {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // 비 키네마틱 상태로 변경
    public void changeNonKinematic(bool useGravity = true) {
        rb.useGravity = useGravity;
        rb.isKinematic = false;
    }

    public bool IsKinematic() {
        return rb.isKinematic;
    }

    private void OnCollisionEnter(Collision other) {
        // 다른 컨테이너랑 충돌 시 이동 초기화
        if(other.gameObject.CompareTag("Container") || other.gameObject.CompareTag("Player")) {
            robotArm.ResetMove();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Wall")) {
            robotArm.ResetMove();
        }

        if(other.gameObject.CompareTag("Floor")) {
            changeKinematic();
        }
    }
}
