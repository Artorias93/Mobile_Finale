using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Text scoreText;
    [SerializeField] protected Slider timerSlider;
    [SerializeField] protected Button pauseBtn;
    [SerializeField] protected EndGameScreenController endGameScreenController;
    [SerializeField] protected PauseGameScreenController pauseGameScreenController;

    protected abstract void LateUpdate();

    protected void UpdateSlider(float value)
    {
        if(value >= 0)
        {
            timerSlider.value = value;
        }
    }

    public void DisableAll()
    {
        pauseBtn.interactable = false;
    }

    public void EnableAll()
    {
        pauseBtn.interactable = true;
    }
    public void HideScore() {
        scoreText.enabled = false;
    }

    public void ShowScore()
    {
        scoreText.enabled = true;
    }
}
