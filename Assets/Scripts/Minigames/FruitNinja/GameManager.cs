using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FruitNinjaMinigame
{
    public class GameManager : MonoBehaviour
    {
        public Timer timer;
        public TMP_Text scoreText;
        public TMP_Text timeText;
        public GameObject obj_blade;
        public int i_outCount = 0;
        public GameObject obj_lose1;
        public GameObject obj_lose2;
        public GameObject obj_lose3;



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
            
            //3-Error Setting
            if (i_outCount == 1) obj_lose1.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
            if (i_outCount == 2) obj_lose2.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
            if (i_outCount == 3)
            {
                obj_lose3.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
                EndGame();
            }
        }

        void EndGame()
        {
            print("GAME OVER!!!");
        }

    }
}
