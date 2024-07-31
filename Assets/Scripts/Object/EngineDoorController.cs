
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EngineDoorController : MonoBehaviour
{
    public Animator anim;
    public EngineScreenController engineScreen;

    private void Update()
    {
        if (engineScreen.isPasswordSucceed)
        {
            anim.SetBool("character_nearby", true);
        }
        else
        {
            anim.SetBool("character_nearby", false);
        }
    }
}
