using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace DecodeAudioMinigame
{
    [System.Serializable]
    public struct RuneEntry
    {
        public string m_runeText;
        public Sprite m_sprite;
        public AudioClip m_audioClip;
    }

    public class DecodeAudioManager : MonoBehaviour
    {
        [Header("Game Parameters")]
        [SerializeField] private RuneEntry[] m_runeDictionary;
        [SerializeField] private int m_turnNumber = 3;
        [SerializeField] private int m_runeCount = 4;
        [SerializeField, Range(0f, 20f)] private float m_playTime = 5f;

        [Header("References")]
        [SerializeField] private BookManager m_bookManager;
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private endGameScreenController m_endGameScreenController;
        [SerializeField] private DecodeAudioUIManager m_decodeUIManager;

        public static DecodeAudioManager m_instance;

        private List<RuneEntry> m_selectedRunes;
        private int m_correctIndex;
        private int m_currentTurn = 1;
        private Timer gameTimer;

#region PROPERTIES
        public int TurnNumber
        {
            get { return m_turnNumber; }
        }
        public int currentTurn
        {
            get { return m_currentTurn; }
        }
        public float PlayTime
        {
            get { return m_playTime; }
        }
        public float RemainingTime
        {
            get { return gameTimer ? gameTimer.RemainingTime : 0; }
        }
#endregion
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Assert.IsNotNull(m_bookManager);
            Assert.IsNotNull(m_endGameScreenController);
            Assert.IsNotNull(m_decodeUIManager);
            Assert.IsFalse(m_runeDictionary.Length < m_runeCount, "Impossible to populate screen, to little runes");
        }

        private void generateRound()
        {
            m_selectedRunes = new List<RuneEntry>();
            while (m_selectedRunes.Count < m_runeCount)
            {
                int index = Random.Range(0, m_runeCount);
                RuneEntry entry = m_runeDictionary[index];
                if (!m_selectedRunes.Contains(entry))
                {
                    m_selectedRunes.Add(entry);
                }
            }

            m_correctIndex = Random.Range(0, m_selectedRunes.Count);
            m_bookManager.UpdateRunes(m_selectedRunes);
            playCorrectClip();
        }

        private void Start()
        {
            m_bookManager.Initialize(m_runeCount);
            generateRound();

            gameTimer = Timer.Create(m_playTime, timerHasEnded, name: "gameTimer");
            gameTimer.StartTimer();
        }

        public void playCorrectClip()
        {
            m_audioSource.PlayOneShot(m_selectedRunes[m_correctIndex].m_audioClip);
        }

        private void timerHasEnded()
        {
            //Logic to play when the timer finishes and the player still hasn't won the game;
            //This is the only designed losing condition right now
            m_bookManager.DisableAll();
            m_decodeUIManager.DisableAll();
            m_endGameScreenController.showLosingScreen();
        }

        private void victoryBehaviour()
        {
            //Activate UI and Logic for winning Game
            m_bookManager.DisableAll();
            m_decodeUIManager.DisableAll();
            m_endGameScreenController.showWinningScreen();
        }

        public void checkEntry(RuneEntry re)
        {
            if (re.Equals(m_selectedRunes[m_correctIndex]))
            {
                Debug.Log("Giusto!!!!");
                m_currentTurn++;
                if (m_currentTurn > m_turnNumber)
                {
                    //End Minigame
                    gameTimer?.Destroy();
                    victoryBehaviour();
                }
                else
                {
                    generateRound();
                }
            }
            else
            {
                Debug.Log("Sbagliato!!!!");
                //Bad Logic
            }
        }
    }
}
