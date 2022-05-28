using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EscapeDungeonMinigame
{
    

    public class EnemyController : MonoBehaviour
    {
        [System.Serializable]
        enum SearchAlgorithm
        {
            Naive_Manhattan, BFS,
        }

        [Header("References")]
        [SerializeField] private MazeManager m_mazeManager;
        [Header("Options")]
        [SerializeField] private SearchAlgorithm m_searchAlgorithm = SearchAlgorithm.BFS;

        private Tile m_currentTile;
        private Tile m_requestedTile;

        public Vector2Int Position { get { return m_currentTile.Coords; } }

        private bool b_isDisabled = false;

        private void Awake()
        {
            Assert.IsNotNull(m_mazeManager, "No maze manager");
        }

        void Start()
        {
            EscapeMinigameManager.OnUpdate += UpdatePosition;
        }

        private void AlgorithmSelection()
        {
            switch (m_searchAlgorithm)
            {
                case SearchAlgorithm.Naive_Manhattan:
                    NaiveManhattanDistance();
                    break;
                case SearchAlgorithm.BFS:
                    BreadthFirstSearch();
                    break;
                default:
                    break;

            };
        }

        void UpdatePosition()
        {

            AlgorithmSelection();

            if (!b_isDisabled && m_requestedTile != null && !m_requestedTile.IsOccupied)
            {
                transform.position = m_requestedTile.transform.position;
                m_currentTile.SetOccupied(false);
                m_currentTile = m_requestedTile;
                m_currentTile.SetOccupied(true);
                m_requestedTile = null;
            }
        }

        public void SetPosition(Tile t)
        {
            Debug.Log("Requested movement to " + t);
            if (t.IsOccupied)
            {
                Debug.Log("Trying to move to occupied tile");
                return;
            }
            else if (t.ReachableTiles.Contains(m_currentTile))
            {
                m_requestedTile = t;
            }
            else
            {
                Debug.Log("Requested " + t + " but not reacheable");
            }
        }

        public void Spawn(Tile t)
        {
            m_currentTile = t;
            m_currentTile.SetOccupied(true);
            transform.position = t.transform.position;
        }

        public void DisableEnemyMovement()
        {
            b_isDisabled = true;
        }

        public void EnableEnemyMovement()
        {
            b_isDisabled = false;
        }

        private void BreadthFirstSearch()
        {
            if (m_currentTile == null)
                return;
            else
            {
                Tile playerTile = PlayerController.Instance.CurrentTile;

                Queue<Tile> stack = new Queue<Tile>();
                HashSet<Tile> visited = new HashSet<Tile> ();
                Dictionary<Tile, Tile> parentPaths = new Dictionary<Tile, Tile> ();

                stack.Enqueue(m_currentTile);
                parentPaths.Add (m_currentTile, null);
                visited.Add (m_currentTile);

                while(stack.Count > 0)
                {
                    Tile t = stack.Dequeue();
                    if (t.Equals(playerTile))
                    {
                        Debug.Log("Found way");
                        break;
                    }

                    foreach (Tile v in t.ReachableTiles)
                    {
                        if (!visited.Contains(v))
                        {
                            stack.Enqueue(v);
                            visited.Add(v);
                            parentPaths[v] = t;
                        }
                    }
                }

                //Find Next move
                Tile next = playerTile;
                while (next != null && parentPaths[next] != m_currentTile)
                {
                    next = parentPaths[next];
                }
                    

                m_requestedTile = next;
            }
        }

        private void NaiveManhattanDistance()
        {
            Tile minimumDistanceTile = null;
            float minimumDistance = float.MaxValue;

            Vector2Int PlayerCoords = PlayerController.Instance.Position;


            foreach(Tile t in m_currentTile.ReachableTiles)
            {
                float distance = Mathf.Abs(PlayerCoords.x - t.Coords.x ) + Mathf.Abs(PlayerCoords.y - t.Coords.y );
                if(minimumDistance > distance)
                {
                    minimumDistance = distance;
                    minimumDistanceTile = t;
                }
            }
            m_requestedTile = minimumDistanceTile;
        }
    }
}
