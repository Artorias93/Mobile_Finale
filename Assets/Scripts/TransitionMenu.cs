using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionMenu : MonoBehaviour
{
    [SerializeField] private float f_animSpeed = 1.5f;
    private float f_exitSpeed;
    private Animator anim;
    private GameObject objToActive;
    private GameObject objToDeactive;
    private GameObject childObjToDeactive;
    private GameObject childObjToActive;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("AnimSpeed", f_animSpeed);
        anim.SetBool("Ready", true);
        f_exitSpeed = anim.GetCurrentAnimatorStateInfo(0).length;
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

    public void StartOutTransition(GameObject objSetter0)
    {
        objToDeactive = objSetter0;
        anim.SetTrigger("Out");
        StartCoroutine(WaitToDeactive());
    }

    private IEnumerator WaitToDeactive()
    {
        yield return new WaitForSeconds(f_exitSpeed);

        if (!gameObject.CompareTag("MainMenu"))
        {
            if (anim != null)
            {
                if (anim.GetBool("ToMenu"))
                {
                    childObjToDeactive.SetActive(false);
                    childObjToActive.SetActive(true);
                }
            }
        }

        StartInTransition();
    }
    private void StartInTransition()
    {
        objToDeactive.SetActive(false);
        if (objToActive != null)objToActive.SetActive(true);
    }

}
