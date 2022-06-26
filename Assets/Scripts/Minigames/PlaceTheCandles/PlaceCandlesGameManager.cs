using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PlaceTheCandles
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public int i_runeToComplete;
    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class PlaceCandlesGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private List<GameObject> rg_runes;
        [SerializeField] private GameObject obj_spawnCandles;
        [SerializeField] private GameObject obj_candles;

        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;


        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Diff diff;

        private GameParameters m_gameParameters;
        private Timer m_gameTimer;

        Animator anim;
        GameObject obj_rune;
        int i_runeCompleted = 0;
        int i_candlesPlaced = 0;
        int i_candlesCount;
        int i_childToCheckId;

        private bool b_gameWon = false;

        public static PlaceCandlesGameManager Instance;

#region PROPERTIES

        public bool GameWon
        {
            get { return b_gameWon; }
        }

        public int RunesCount
        {
            get { return i_runeCompleted; }
        }

        public bool CandleCheck
        {
            get { return i_candlesPlaced == i_candlesCount; }
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

        private void Start()
        {
            if (b_enableDebug)
            {
                if (diff == Diff.easy) BeginMinigame(Difficulty.EASY);
                else if (diff == Diff.medium) BeginMinigame(Difficulty.MEDIUM);
                else if (diff == Diff.hard) BeginMinigame(Difficulty.HARD);
            }
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (difficulty == Difficulty.EASY)
            {
                m_gameParameters = gp_easy;
                i_childToCheckId = 0;
            }
            else if (difficulty == Difficulty.MEDIUM)
            {
                m_gameParameters = gp_medium;
                i_childToCheckId = 1;
            }
            else if (difficulty == Difficulty.HARD)
            {
                m_gameParameters = gp_hard;
                i_childToCheckId = 2;
            }

            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();

            GenerateRound();
        }

        private void GenerateRound()
        {
            i_candlesPlaced = 0;
            i_candlesCount = 0;
            int i_runeId = Random.Range(0, rg_runes.Count);
            obj_rune = Instantiate(rg_runes[i_runeId], new Vector3(0, -0.35f, 1), Quaternion.identity);
            anim = obj_rune.transform.GetChild(3).GetComponent<Animator>();
            rg_runes.RemoveAt(i_runeId);

            for (int i = 0; i <= i_childToCheckId; i++)
            {
                obj_rune.transform.GetChild(i).gameObject.SetActive(true);
                i_candlesCount += obj_rune.transform.GetChild(i).childCount;
            }

            SpawnCandles(i_candlesCount);
        }

        private void SpawnCandles(int i_candlesNum)
        {
            for (int i = 0; i <= i_candlesNum - 1; i++)
            {
                GameObject obj_candle = Instantiate(obj_candles);
                obj_candle.transform.SetParent(obj_spawnCandles.transform.GetChild(i).transform);
                obj_candle.transform.localPosition = Vector3.zero;
            }
        }

        IEnumerator WaitGenerate()
        {
            if (i_runeCompleted == GameParameters.i_runeToComplete)
            {
                VictoryBehaviour();
                yield break;
            }

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            GenerateRound();
        }

        public void CountRunes()
        {
            i_runeCompleted++;
            anim.SetTrigger("Destroy");
            Destroy(obj_rune, anim.GetCurrentAnimatorStateInfo(0).length);
            StartCoroutine(WaitGenerate());
        }

        public void CountCandles()
        {
            i_candlesPlaced++;
            if (i_candlesPlaced == i_candlesCount) CountRunes();
        }

        public void LoseBehaviour()
        {
            EndMinigame();
        }

        protected override void EndMinigame()
        {
            m_gameTimer?.StopTimer();
            b_gameFinished = true;

            i_score = 3 - Mathf.FloorToInt((Mathf.Abs(((float)i_runeCompleted / (float)m_gameParameters.i_runeToComplete * 100) - 100) / 100 * 4));
            if (i_score > 3) i_score = 3;

        }

        private void VictoryBehaviour()
        {
            b_gameWon = true;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            LoseBehaviour();
        }

    }
}

