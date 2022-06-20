using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float f_fadeSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void StartInTransition()
    {
        GetComponent<Animator>().SetTrigger("In");
    }
    public void StartOutTransition()
    {
        GetComponent<Animator>().SetTrigger("Out");
    }

}
