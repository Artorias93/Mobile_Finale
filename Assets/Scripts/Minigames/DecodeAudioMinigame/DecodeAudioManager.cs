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

    [System.Serializable]
    public struct GameParameters
    {
        public int m_turnNumber;
        public int m_runeCount;
        public float m_playTime;
    }

    public class DecodeAudioManager : AMinigameManager
    {
        [Header("Game Parameters")]
        [SerializeField] private RuneEntry[] m_runeDictionary;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters m_gameParameters;

        [Header("References")]
        [SerializeField] private BookManager m_bookManager;
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private EndGameScreenController m_endGameScreenController;
        [SerializeField] private DecodeAudioUIManager m_decodeUIManager;

        public static DecodeAudioManager m_instance;

        private List<RuneEntry> m_selectedRunes;
        private int m_correctIndex;
        private int m_currentTurn = 1;
        private Timer gameTimer;

#region PROPERTIES
        public int TurnNumber
        {
            get { return m_gameParameters.m_turnNumber; }
        }
        public int currentTurn
        {
            get { return m_currentTurn; }
        }
        public float PlayTime
        {
            get { return m_gameParameters.m_playTime; }
        }

        public float RemainingTime
        {
            get { return gameTimer ? gameTimer.RemainingTime : -1; }
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
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;

            if (m_difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (m_difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (m_difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            m_bookManager.Initialize(m_gameParameters.m_runeCount);
            generateRound();

            gameTimer = Timer.Create(m_gameParameters.m_playTime, timerHasEnded, name: "gameTimer");
            gameTimer.StartTimer();
        }

        public override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }

        private void generateRound()
        {
            m_selectedRunes = new List<RuneEntry>();
            while (m_selectedRunes.Count < m_gameParameters.m_runeCount)
            {
                int index = Random.Range(0, m_gameParameters.m_runeCount);
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

        
        public void playAudioClip(AudioClip clip)
        {
            m_audioSource.Stop();
            m_audioSource.PlayOneShot(clip);
        }

        private void playCorrectClip()
        {
            m_audioSource.Stop();
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
            playAudioClip(re.m_audioClip);
            if (re.Equals(m_selectedRunes[m_correctIndex]))
            {
                Debug.Log("Giusto!!!!");
                m_currentTurn++;
                if (m_currentTurn > m_gameParameters.m_turnNumber)
                {
                    //End Minigame
                    m_currentTurn = m_gameParameters.m_turnNumber; //To avoid UI to show wrong number;
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

        public override void PauseGame()
        {
            base.PauseGame();
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
        }
    }
}
