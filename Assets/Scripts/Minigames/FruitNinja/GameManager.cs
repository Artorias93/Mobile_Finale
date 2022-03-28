using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Timer timer;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public GameObject obj_blade;


    // Start is called before the first frame update
    void Start()
    {
        timer.Initialize(5);
        Timer.OnTimeEnd += EndGame;

    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = timer.Minutes.ToString("D2") + ":" + timer.Seconds.ToString("D2");
        scoreText.text = obj_blade.GetComponent<Blade>().i_scoring.ToString();
    }

    void EndGame()
    {
        print("END TIME!!!");
    }

}
