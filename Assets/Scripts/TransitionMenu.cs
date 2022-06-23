using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionMenu : MonoBehaviour
{
    [SerializeField] private float f_animSpeed = 1;
    private Animator anim;
    private GameObject objToActive;
    private GameObject childObjToDeactive;
    private GameObject childObjToActive;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("AnimSpeed", f_animSpeed);
    }

    public void SetNext(bool b_isNext)
    {
        anim.SetBool("Next", b_isNext);
    }

    public void SetObjToActive(GameObject objSetter1)
    {
        objToActive = objSetter1;
    }

    public void SetChildObjToDeactive(GameObject objSetter2)
    {
        childObjToDeactive = objSetter2;
    }
    public void SetChildObjToActive(GameObject objSetter3)
    {
        childObjToActive = objSetter3;
    }

    public void StartOutTransitionMenu()
    {
        anim.SetTrigger("Menu");
        anim.SetBool("ToMenu", true);
    }

    public void StartOutTransition(GameObject objToDeactive)
    {
        anim.SetTrigger("Out");
        StartCoroutine(WaitToDeactive(objToDeactive));
    }

    private IEnumerator WaitToDeactive(GameObject objToDeactive)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        if (!gameObject.CompareTag("MainMenu"))
        {
            if (anim.GetBool("ToMenu"))
            {
                childObjToDeactive.SetActive(false);
                childObjToActive.SetActive(true);
            }
        }

        objToDeactive.SetActive(false);
        StartInTransition();
    }
    private void StartInTransition()
    {
        objToActive.SetActive(true);
    }

}
