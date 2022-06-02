using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator m_animator;

    [Header("Animation parameters")]
    [SerializeField] private string m_loadingTrigger = "StartedLoading";
    [SerializeField] private string m_finishedLoadingTrigger = "Ready";

    private bool b_AnimationCompleted;

    public bool AnimationCompleted {  get { return b_AnimationCompleted; } }

    // Start is called before the first frame update
    private void Awake()
    {
        Assert.IsNotNull(m_animator, "No animator referenced");
    }

    public void StartLoadingTransition()
    {
        b_AnimationCompleted = false;
        m_animator.ResetTrigger(m_finishedLoadingTrigger);
        m_animator.SetTrigger(m_loadingTrigger);
        
    }

    public void StartReadyTransition()
    {
        b_AnimationCompleted = false;
        m_animator.ResetTrigger(m_loadingTrigger);

        m_animator.SetTrigger(m_finishedLoadingTrigger);

    }

    public void SetComplete() { b_AnimationCompleted = true; }
}
