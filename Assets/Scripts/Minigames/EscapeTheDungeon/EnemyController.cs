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
        [SerializeField, Tooltip("Probability that if the enemy is far enough it will just patrol randomly instead of moving toward the mage")
            , Range(.0f, 1.0f)] 
        private float f_probabilityOfRandomStep = 0.8f;
        [SerializeField, Tooltip("Probability that if the enemy is chasing he will pick a bad move"), Range(.0f, 1.0f)]
        private float f_probabilityOfMistakeChase = 0.1f;
        [SerializeField, Tooltip("Probability of backtracking"), Range(.0f, 1.0f)]
        private float f_probabilityOfBacktracking = 0.2f;
        [SerializeField, Tooltip("Minimum distance after which the enemy will never do a wrong move")]
        private int i_minimumDistanceBeforeChase = 5;
        private int i_StepsFromPlayer = int.MaxValue;

        private Tile m_currentTile;
        private Tile m_requestedTile;
        private Tile m_previousTile = null;

        public Vector2Int Position { get { return m_currentTile.Coords; } }

        private bool b_isDisabled = false;
        private bool b_AttackedPlayer = false;
        public bool AttackedPlayer { get { return b_AttackedPlayer; } }

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
            if (!b_isDisabled)
            {
                AlgorithmSelection();   //This needs to be called before TileRandomization to update steps from player
                TileRandomization();

                if (m_requestedTile != null && !m_requestedTile.IsOccupied)
                {
                    transform.position = m_requestedTile.transform.position;
                    m_currentTile.SetOccupied(false);
                    m_previousTile = m_currentTile;
                    m_currentTile = m_requestedTile;
                    m_currentTile.SetOccupied(true);
                    m_requestedTile = null;
                } else if(m_requestedTile.Equals(PlayerController.Instance.CurrentTile) && i_StepsFromPlayer == 0)
                {
                    //I'm adjacent to player, moving toward him means I'm attacking
                    b_AttackedPlayer = true;
                }
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
                i_StepsFromPlayer = 0;

                Tile next = playerTile;
                while (next != null && parentPaths[next] != m_currentTile)
                {
                    i_StepsFromPlayer++;
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
            i_StepsFromPlayer = Mathf.RoundToInt(minimumDistance);
        }

        private void TileRandomization()
        {
            //Decides if using selected "optimal" tile or randomize
            if(i_StepsFromPlayer > i_minimumDistanceBeforeChase)
            {
                float randomValue = Random.Range(0.0f, 1.0f);
                if (randomValue < f_probabilityOfRandomStep)
                {
                    Debug.Log(name + " decides to randomize");
                    GetRandomAdjacentTile();
                    return;
                }
            } else if(i_StepsFromPlayer < i_minimumDistanceBeforeChase)
            {
                float randomValue = Random.Range(0.0f, 1.0f);
                if(randomValue < f_probabilityOfMistakeChase)
                {
                    Debug.Log(name + " makes a mistake chasing");
                    GetRandomAdjacentTile();
                    return;
                }
                
            }
            Debug.Log(name + " picks optimal");
        }

        private void GetRandomAdjacentTile()
        {
            if(m_currentTile.ReachableTiles.Count > 2) //I need at least 2 tiles to pick one random with backtracking
            {

                Tile suboptimalTile = m_requestedTile;
                Tile[] possibleTiles = new Tile[m_currentTile.ReachableTiles.Count];
                m_currentTile.ReachableTiles.CopyTo(possibleTiles);
                do
                {
                    int randomIndex = Random.Range(0, possibleTiles.Length);
                    suboptimalTile = possibleTiles[randomIndex];

                    float randomBacktracking = Random.Range(0.0f, 1.0f);
                    if(randomBacktracking > f_probabilityOfBacktracking && suboptimalTile.Equals(m_previousTile))
                    {
                        continue;
                    }
                } while (m_requestedTile == suboptimalTile);
                m_requestedTile = suboptimalTile;
            }
        }
    }
}
