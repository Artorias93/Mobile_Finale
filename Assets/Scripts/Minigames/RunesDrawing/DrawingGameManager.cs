using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DrawingRunes
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_maxTurns;
        public int i_playTime;
    }

    [System.Serializable]
    public struct Shape
    {
        public string name;
        public Vector3[] vertices;
    }
    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class DrawingGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private TouchManager m_touchManager;
        [SerializeField] private LineRenderer m_lineRenderer;

        [Header("Game params")]
        [SerializeField, Tooltip("Regulates how different shapes can be, interpolation and smoothness setting of touch manager will alter the optimal value")]
        private float f_Tollerance = 1.5f;
        [SerializeField, Tooltip("might increase precision, a value of 1 or 2 is suggested"), Range(0, 5)]
        private int i_distanceExponent = 2;
        [SerializeField] private Shape[] m_shapes;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;

        [Header("Debug functionalities")]
        [SerializeField] private bool b_enableDebug = false;
        [SerializeField] private Diff diff;
        [SerializeField, Min(0.0f)] private int i_debugShape;
        [SerializeField, Range(0, 1)] private float f_debugGizmosSize;

        private GameParameters m_gameParameters;
        private int i_currentTurn = 1;
        private Timer m_gameTimer;
        private bool[] b_pickedIndexes; //Used to prevent drawing the same shape over and over
        private bool b_gameWon = false;

        public static DrawingGameManager Instance;

#region PROPERTIES

        public bool GameWon
        {
            get { return b_gameWon; }
        }

        public int CurrentTurn
        {
            get { return i_currentTurn; }
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

            Assert.IsNotNull(m_touchManager, "no touch manager found");
            Assert.IsNotNull(m_lineRenderer, "no line renderer found");
            Assert.IsFalse(m_shapes == null || m_shapes.Length == 0, "No shape in list");

            b_pickedIndexes = new bool[m_shapes.Length];
            i_score = 0;
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
                m_gameParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            generateRound();
            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();
        }

        protected override void EndMinigame()
        {
            b_gameFinished = true;
            i_score = (int) ( 3 * (float) i_score / m_gameParameters.i_maxTurns);
        }

        private void VictoryBehaviour()
        {
            m_gameTimer?.StopTimer();
            i_currentTurn = m_gameParameters.i_maxTurns;
            b_gameWon = true;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            EndMinigame();
        }
        
        void generateRound()
        {
            
            int randomShape = Random.Range(0, m_shapes.Length);
            while(b_pickedIndexes[randomShape]){
                randomShape = Random.Range(0, m_shapes.Length);
            }

            b_pickedIndexes[randomShape] = true;
            bool allPicked = true;
            foreach(bool isIndexPicked in b_pickedIndexes)
            {
                if (!isIndexPicked)
                {
                    allPicked = false;
                    break;
                }
            }
            if (allPicked)
            {
                for (int i = 0; i < b_pickedIndexes.Length; i++)
                    b_pickedIndexes[i] = false;
            }


            Debug.Log("Picked " + m_shapes[randomShape].name);
            m_touchManager.requestShape(new List<Vector3>(m_shapes[randomShape].vertices));

            //Draw reference
            m_lineRenderer.positionCount = m_shapes[randomShape].vertices.Length;
            m_lineRenderer.SetPositions(m_shapes[randomShape].vertices);
        }

        void LateUpdate()
        {
            if (!b_gameFinished)
            {
                DrawingStatus status = m_touchManager.Status;
                if (status == DrawingStatus.FINISHED)
                {
                    float distance = m_touchManager.Distance;
                    distance = Mathf.Pow(distance, i_distanceExponent);

                    Debug.Log("Distance: " + distance + " Tollerance: " + f_Tollerance);
                    if (distance <= f_Tollerance)
                    {
                        Debug.Log("Round won");
                        i_currentTurn++;
                        i_score++;
                        if (i_currentTurn > m_gameParameters.i_maxTurns)
                        {
                            VictoryBehaviour();
                        }
                        else
                        {
                            generateRound();
                        }
                    } else
                    {
                        //Give feedback on error
                        Debug.Log("Shape is not correct");
                    }
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //This code helps on editing shapes
            if (b_enableDebug && m_shapes != null && i_debugShape >= 0 && i_debugShape < m_shapes.Length && m_shapes[i_debugShape].vertices != null)
            {
                Vector3[] selectedVertices = (Vector3[]) m_shapes[i_debugShape].vertices.Clone();

                for(int i = 0; i< selectedVertices.Length - 1; i++)
                {

                    Gizmos.DrawSphere(selectedVertices[i], f_debugGizmosSize);
                    Gizmos.DrawLine(selectedVertices[i], selectedVertices[i + 1]);
                }
                int lastIndex = selectedVertices.Length - 1;

                Gizmos.DrawSphere(selectedVertices[lastIndex], f_debugGizmosSize);
                Gizmos.DrawLine(selectedVertices[lastIndex], selectedVertices[0]);
            }
                
        }
#endif
    }
}

