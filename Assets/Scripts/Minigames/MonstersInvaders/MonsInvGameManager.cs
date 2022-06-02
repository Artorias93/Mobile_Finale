using UnityEngine;
using UnityEngine.UI;

namespace MonstersInvaders
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public int i_enemiesRows;
        public float f_monsterSpeedMultiplier;
        public float f_monsterFireRateMultiplier;
        public float f_monsterAttackSpeedMultiplier;
        public int i_maxError;
    }

    public class MonsInvGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private GameObject[] rg_monsters;
        [SerializeField] private GameObject obj_boss;
        [SerializeField] private GameObject obj_bossSpawnPoint;
        [SerializeField] private GameObject obj_spawnGrid;

        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;

        private GameParameters m_gameParameters;
        private Timer m_gameTimer;

        int i_numError = 0;
        private bool b_floorColl;
        private bool b_spawnBoss = true;
        private bool b_isTimeEnded = false;

        private bool b_gameWon = false;

        public static MonsInvGameManager Instance;

#region PROPERTIES
        public bool GameWon
        {
            get { return b_gameWon; }
        }

        public int ErrorCount
        {
            get { return i_numError; }
        }

        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public GameParameters GameParameters
        {
            get { return m_gameParameters; }
        }
        #endregion


        private void Awake()
        {
            //BeginMinigame(Difficulty.EASY);       //TEST
            //BeginMinigame(Difficulty.MEDIUM);     //TEST
            //BeginMinigame(Difficulty.HARD);       //TEST

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (obj_spawnGrid.transform.childCount == 0) VictoryBehaviour();

            if (RemainingTime <= GameParameters.i_playTime/2 && b_spawnBoss)
            {
                b_spawnBoss = false;
                GameObject obj_monster = Instantiate(obj_boss);
                obj_monster.transform.SetParent(obj_bossSpawnPoint.transform);
                obj_monster.transform.localPosition = Vector3.zero;
                EnemyParams(obj_monster);
            }
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            for (int i = 0; i < m_gameParameters.i_enemiesRows * 10; i++)
            {
                int i_monsterID = Random.Range(0, rg_monsters.Length);
                GameObject obj_monster = Instantiate(rg_monsters[i_monsterID]);
                obj_monster.transform.SetParent(obj_spawnGrid.transform);
                EnemyParams(obj_monster);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(obj_spawnGrid.GetComponent<RectTransform>());
            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();
            obj_spawnGrid.GetComponent<GridLayoutGroup>().enabled = false;
        }

        private void EnemyParams(GameObject obj_monster)
        {
            obj_monster.GetComponent<Enemy>().f_speedMultiplier = m_gameParameters.f_monsterSpeedMultiplier;
            obj_monster.GetComponent<Enemy>().f_fireRateMultiplier = m_gameParameters.f_monsterFireRateMultiplier;
            obj_monster.GetComponent<Enemy>().f_attackSpeedMultiplier = m_gameParameters.f_monsterAttackSpeedMultiplier;
        }

        public void CountError(int i_errSum)
        {
            i_numError += i_errSum;

            if (i_numError >= m_gameParameters.i_maxError) LoseBehaviour(false);
            if (i_numError < 0) i_numError = 0;
        }

        public void LoseBehaviour(bool b_check)
        {
            b_floorColl = b_check;
            EndMinigame();
        }

        protected override void EndMinigame()
        {
            m_gameTimer?.StopTimer();
            b_gameFinished = true;

            if (b_floorColl) i_score = 0;
            else i_score = Mathf.FloorToInt((Mathf.Abs(((float)i_numError / (float)m_gameParameters.i_maxError * 100) - 100) / 100 * 4));
            if (i_score > 3) i_score = 3;

            if (b_isTimeEnded || i_score > 0) i_score--;
        }

        private void VictoryBehaviour()
        {
            b_gameWon = true;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            b_isTimeEnded = true;
            LoseBehaviour(false);
        }

    }
}

