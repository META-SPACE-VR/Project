using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void PlayBtn()
    {
        SceneManager.LoadScene("Scenes/Stage_1/Level_1");
    }
}
