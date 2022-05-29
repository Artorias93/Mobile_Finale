using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EscapeDungeonMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_PlayTime;
        public int i_numEnemies;
    }

    public class EscapeMinigameManager : AMinigameManager
    {
        [Header("Game Settings")]
        [SerializeField] private float frameTime = 0.5f;
        [SerializeField] private GameObject m_EnemyPrefab;
        [SerializeField] private MazeManager m_MazeManager;
        [SerializeField] private PlayerController m_PlayerController;

        [Header("Difficulty Settings")]
        [SerializeField] private GameParameters gp_easy;
        [SerializeField] private GameParameters gp_medium;
        [SerializeField] private GameParameters gp_hard;

        private GameParameters gp_selectedParameters;

#if UNITY_EDITOR
        [Header("DEBUG")]
        public bool b_debug = false;
        public Difficulty m_TestDifficulty;
#endif

        private Timer m_Timer;
        public static EscapeMinigameManager Instance;

        public delegate void UpdateEvent();
        public static event UpdateEvent OnUpdate;

        private float f_elapsedTime;
        private List<EnemyController> m_EnemyControllers;

        #region PROPERTIES
        public float RemainingTime
        {
            get { return m_Timer ? m_Timer.RemainingTime : -1; }
        }

        public GameParameters SelectedParameters
        {
            get { return gp_selectedParameters; }
        }

        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);

            Assert.IsNotNull(m_MazeManager);
            Assert.IsNotNull(m_EnemyPrefab);
            Assert.IsNotNull(m_PlayerController);

        }

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            if (b_debug) {
                BeginMinigame(m_TestDifficulty);
            }
#endif
        }

        // Update is called once per frame
        void Update()
        {
            f_elapsedTime += Time.deltaTime;
            if(f_elapsedTime > frameTime)
            {
                f_elapsedTime -= frameTime;
                if(OnUpdate != null)
                    OnUpdate();

                if (m_PlayerController.CurrentTile.Equals(m_MazeManager.Goal))
                {
                    VictoryBehaviour();
                }
                else
                {
                    foreach(EnemyController enemy in m_EnemyControllers)
                    {
                        if (enemy.AttackedPlayer)
                        {
                            DeathByEnemy();
                        }
                    }
                }
            }

        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            if (difficulty == Difficulty.EASY)
                gp_selectedParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                gp_selectedParameters = gp_medium;
            else if(difficulty == Difficulty.HARD)
                gp_selectedParameters = gp_hard;

            m_EnemyControllers = new List<EnemyController>();

            //Used to force correct positionitiong of player
            m_MazeManager.CallLayoutRebuilder();
            PlayerController.Instance.Spawn(m_MazeManager.GetTile(m_MazeManager.Height - 1, m_MazeManager.Width-1));

            for (int i = 0; i < gp_selectedParameters.i_numEnemies; i++)
            {
                GameObject enemyGO = Instantiate(m_EnemyPrefab, m_MazeManager.transform);
                enemyGO.name = "Enemy " + i.ToString();
                m_EnemyControllers.Add(enemyGO.GetComponent<EnemyController>());
                m_EnemyControllers[i].Spawn(m_MazeManager.GetRandomFreeTile());
            }


            m_Timer = Timer.Create(gp_selectedParameters.f_PlayTime, OnTimerEnd, name: "MinigameTimer");
            m_Timer.StartTimer();
        }

        protected override void EndMinigame()
        {
            foreach (EnemyController enemyController in m_EnemyControllers)
            {
                enemyController.DisableEnemyMovement();
            }
            m_PlayerController.DisableMovement();

            b_gameFinished = true;
        }

        public void VictoryBehaviour()
        {
            m_Timer?.PauseTimer();

            float normalizedTime = (float)RemainingTime / gp_selectedParameters.f_PlayTime;

            i_score = (int) (3 * normalizedTime) + 1;

            EndMinigame();
        }

        public void OnTimerEnd()
        {
            i_score = 0;

            EndMinigame();
        }

        public void DeathByEnemy()
        {
            i_score = 0;
            m_Timer.PauseTimer();
            EndMinigame();
        }
    }
}

