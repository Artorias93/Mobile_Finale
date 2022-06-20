using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTraining : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartPage1In()
    {
        GetComponent<Animator>().SetTrigger("Page1");
    }

    public void StartPage2In()
    {
        GetComponent<Animator>().SetTrigger("Page2");
    }
    public void StartMenuIn()
    {
        GetComponent<Animator>().SetTrigger("Menu");
    }

}
