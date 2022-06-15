using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ChangeDifficulty()
    {
        anim.SetInteger("Difficulty", (anim.GetInteger("Difficulty") + 1) % 3);
    }
}
