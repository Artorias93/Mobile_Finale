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

    public class DrawingGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private TouchManager m_touchManager;

        [Header("Game params")]
        [SerializeField, Tooltip("Regulates how different shapes can be, interpolation and smoothness setting of touch manager will alter the optimal value")]
        private float f_Tollerance = 1.5f;
        [SerializeField, Tooltip("might increase precision, a value of 1 or 2 is suggested"), Range(0, 5)]
        private int i_distanceExponent = 2;
        [SerializeField] private Shape[] m_shapes;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;

#if UNITY_EDITOR
        [Header("Debug functionalities")]
        [SerializeField] private bool b_enableDebug = false;
        [SerializeField, Min(0.0f)] private int i_debugShape;
        [SerializeField, Range(0, 1)] private float f_debugGizmosSize;
#endif

        private GameParameters m_gameParameters;
        private int i_currentTurn = 1;
        private Timer m_gameTimer;
        private bool b_gameFinished = false, b_gameWon = false;

        private void Start()
        {
            BeginMinigame(Difficulty.EASY);
            generateRound();
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            if (m_difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (m_difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (m_difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            generateRound();
            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
        }

        public override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }

        public void onTimeEnd()
        {
            b_gameFinished = true;
        }
        
        void generateRound()
        {
            int randomShape = Random.Range(0, m_shapes.Length);

            Debug.Log("Picked " + m_shapes[randomShape].name);
            m_touchManager.requestShape(new List<Vector3>(m_shapes[randomShape].vertices));
        }

        void Awake()
        {
            Assert.IsNotNull(m_touchManager, "no touch manager found");
            Assert.IsFalse(m_shapes == null || m_shapes.Length == 0, "No shape in list");
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
                        if (i_currentTurn > m_gameParameters.i_maxTurns)
                        {
                            i_currentTurn = m_gameParameters.i_maxTurns;
                            b_gameWon = true;
                            b_gameFinished = true;
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


        private void OnDrawGizmos()
        {
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
    }
}

