using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FruitNinjaMinigame
{
    public class GameManager : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private GameObject obj_blade;
        [SerializeField] private int i_outCount = 0;
        [SerializeField] private GameObject obj_lose1;
        [SerializeField] private GameObject obj_lose2;
        [SerializeField] private GameObject obj_lose3;
        [SerializeField] private float gameTime = 5f;

        private Timer m_gameTimer;
        private bool m_gameFinished;
        public bool GameFinished
        {
            get { return m_gameFinished; }
        }

        public static GameManager Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            m_gameFinished = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_gameTimer = Timer.Create(gameTime, EndGame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
        }

        // Update is called once per frame
        void Update()
        {
            timeText.text = m_gameTimer.ConvertedTime.minutes.ToString("D2") + ":" + m_gameTimer.ConvertedTime.seconds.ToString("D2");
            scoreText.text = obj_blade.GetComponent<Blade>().i_scoring.ToString();
            
            
        }

        public void increaseErrorCount()
        {
            i_outCount++;
            if (i_outCount == 1) obj_lose1.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
            if (i_outCount == 2) obj_lose2.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
            if (i_outCount == 3)
            {
                obj_lose3.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 1f);
                if(m_gameTimer != null)
                    m_gameTimer.Destroy();
                EndGame();
            }
        }

        void EndGame()
        {
            m_gameFinished = true;
            print("GAME OVER!!!");
        }

    }
}
