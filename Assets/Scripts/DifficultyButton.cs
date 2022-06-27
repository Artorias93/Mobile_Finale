using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    Animator anim;
    [SerializeField] private int i_currentState = 0;
    [SerializeField] private Color easy;
    [SerializeField] private Color medium;
    [SerializeField] private Color hard;
    [SerializeField] private TransitionMenu transition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        i_currentState= transition.i_difficulty;
        anim.SetInteger("Difficulty", transition.i_difficulty);

        SetColorAndText();
    }
    public void ChangeDifficulty()
    {
        anim.SetInteger("Difficulty", (anim.GetInteger("Difficulty") + 1) % 3);
        i_currentState = anim.GetInteger("Difficulty");

        SetColorAndText();

        transition.i_difficulty = i_currentState;
    }

    private void SetColorAndText()
    {
        switch (i_currentState)
        {
            case 0:
                GetComponent<Image>().color = easy;
                transform.GetChild(1).GetComponent<Text>().text = "Easy";
                break;
            case 1:
                GetComponent<Image>().color = medium;
                transform.GetChild(1).GetComponent<Text>().text = "Medium";
                break;
            case 2:
                GetComponent<Image>().color = hard;
                transform.GetChild(1).GetComponent<Text>().text = "Hard";
                break;
        }
    }

    public void Clicked()
    {
        anim.SetBool("Clicked", !anim.GetBool("Clicked"));
    }

    public Difficulty getSelectedDifficulty() {
        return (Difficulty)i_currentState;
    }

}
