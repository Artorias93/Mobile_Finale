using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EscapeDungeonMinigame
{

    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MazeManager m_mazeManager;

        private Tile m_currentTile;
        private Tile m_requestedTile;

        public Vector2Int Position { get { return m_currentTile.Coords; } }

        public static PlayerController Instance;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            Assert.IsNotNull(m_mazeManager, "No maze manager");
        }

        void Start()
        {
            EscapeMinigameManager.OnUpdate += UpdatePosition;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void UpdatePosition()
        {
            if(m_requestedTile != null)
            {
                transform.position = m_requestedTile.transform.position;
                m_currentTile = m_requestedTile;
                m_requestedTile = null;
            }
        }

        public void SetPosition(Tile t)
        {
            if (t.ReachableTiles.Contains(m_currentTile))
            {
                Debug.Log("Requested movement to " + t);
                m_requestedTile = t;
            } else
            {
                Debug.Log("Requested " + t + " but not reacheable");
            }
        }

        public void Spawn(Tile t)
        {
            m_currentTile = t;
            transform.position = m_currentTile.transform.position;
        }
    }
}
