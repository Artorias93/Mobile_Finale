using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    Animator anim;
    [SerializeField] private int i_currentState = 0;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        //if (anim.GetInteger("Difficulty") == -1) anim.SetInteger("Difficulty", 0);
    }

    private void OnEnable()
    {
        anim.SetInteger("Difficulty", i_currentState);
    }
    public void ChangeDifficulty()
    {

        anim.SetInteger("Difficulty", (anim.GetInteger("Difficulty") + 1) % 3);
        i_currentState = anim.GetInteger("Difficulty");
    }
}
