using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EngineScreenController : MonoBehaviour
{
    public GameObject password;
    public GameObject keyboard;
    public TextMeshProUGUI showText;
    public Camera focusCamera;
    public Material enginePasswordSuccessMaterial;

    private float range = 5.0f;
    private string code = "";
    private Transform[] codeIndicators;

    public bool isPasswordSucceed;

    private void Start()
    {
        codeIndicators = new Transform[password.transform.childCount];
        for (int i = 0; i < codeIndicators.Length; i++)
        {
            codeIndicators[i] = password.transform.GetChild(i);
            codeIndicators[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 controllerForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;

            Ray ray = new Ray(controllerPosition, controllerForward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                GameObject key = hit.collider.gameObject;

                if (key != null && key.transform.IsChildOf(keyboard.transform))
                {
                    string pressedKeyName = key.name;

                    if (pressedKeyName == "BS" && code.Length > 0) // 백스패이스 로직
                    {
                        code = code.Substring(0, code.Length - 1);
                        codeIndicators[code.Length].gameObject.SetActive(false);
                    }
                    else if (pressedKeyName == "ET") // 엔터 로직
                    {
                        if (code == "1036")
                        {
                            Renderer renderer = GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = enginePasswordSuccessMaterial;
                                password.SetActive(false);
                                keyboard.SetActive(false);
                            }
                            else
                            {
                                StartCoroutine(ShowWrongPasswordMessage());
                            }
                        }
                    }
                    else // 나머지 키 로직
                    {
                        if (code.Length < 4)
                        {
                            code += pressedKeyName;
                            codeIndicators[code.Length - 1].gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ShowWrongPasswordMessage()
    {
        showText.text = "패스워드가 틀렸습니다";
        yield return new WaitForSeconds(1);
        showText.text = "";
        code = "";
        foreach (Transform indicator in codeIndicators)
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
