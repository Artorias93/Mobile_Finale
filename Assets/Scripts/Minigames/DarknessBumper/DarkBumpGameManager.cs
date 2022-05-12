using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DarknessBumper
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public float f_skullSpeedMultiplier;
        public float f_minSpawnDelay;
        public float f_maxSpawDelay;
        public float f_timePersistence;
        public int i_maxError;
    }

    public class DarkBumpGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private GameObject[] rg_darkness;
        [SerializeField] private Collider2D spawnBounds;

        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;

        private GameParameters m_gameParameters;
        int i_numError = 0;
        private Timer m_gameTimer;

        private bool b_gameFinished = false, b_gameWon = false;

        public static DarkBumpGameManager Instance;

#region PROPERTIES
        public bool GameFinished
        {
            get { return b_gameFinished; }
        }

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
            BeginMinigame(Difficulty.EASY);     //TEST

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

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            StartCoroutine(SpawnSkulls(m_gameParameters.f_minSpawnDelay, m_gameParameters.f_maxSpawDelay));
            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();
        }

        private IEnumerator SpawnSkulls(float f_minDelay, float f_maxDelay)
        {
            while (true)
            {
                float f_delay = Random.Range(f_minDelay, f_maxDelay);
                int f_skullIndex = Random.Range(0, rg_darkness.Length);

                var bounds = spawnBounds.bounds;
                var f_px = Random.Range(bounds.min.x, bounds.max.x);
                var f_py = Random.Range(bounds.min.y, bounds.max.y);
                Vector2 pos = new Vector3(f_px, f_py);

                yield return new WaitForSeconds(f_delay);

                GameObject obj_skull = Instantiate(rg_darkness[f_skullIndex], pos, transform.rotation);
                obj_skull.GetComponent<Skull>().f_speedMultiplier = m_gameParameters.f_skullSpeedMultiplier;
                obj_skull.GetComponent<Skull>().i_moveType = Random.Range(0, 7);
                obj_skull.GetComponent<Skull>().b_skullDir = Random.Range(0, 2) == 1;
                obj_skull.GetComponent<Skull>().f_persistence = m_gameParameters.f_timePersistence;
                Destroy(obj_skull, m_gameParameters.f_timePersistence + 2f);
            }
        }

        public void CountError()
        {
            i_numError++;
            if (i_numError == m_gameParameters.i_maxError)
            {
                print("LOSER!!!!!!!");                      //TRIGGER SCONFITTA CAUSA ERRORI
                EndMinigame();
            }
        }

        public override void EndMinigame()
        {
            m_gameTimer?.StopTimer();
            Time.timeScale = 0;                             //Giusto o crea problemi?
            b_gameFinished = true;
        }

        private void VictoryBehaviour()
        {
            m_gameTimer?.StopTimer();
            Time.timeScale = 0;                             //Giusto o crea problemi?
            b_gameWon = true;
            b_gameFinished = true;
            print("WINNER!!!!!!!!!");
        }
        private void onTimeEnd()
        {
            VictoryBehaviour();                             //TRIGGER VITTORIA
        }

    }
}

