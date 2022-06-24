using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public void Boot()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
