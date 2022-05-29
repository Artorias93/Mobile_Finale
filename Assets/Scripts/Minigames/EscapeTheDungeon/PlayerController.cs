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
        public Tile CurrentTile { get { return m_currentTile; } }

        private bool b_IsMovementDisabled = false;


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
        void UpdatePosition()
        {
            if(!b_IsMovementDisabled && m_requestedTile != null && !m_requestedTile.IsOccupied)
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
            } else
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

        public void DisableMovement()
        {
            b_IsMovementDisabled = true;
        }

        public void EnableMovement()
        {
            b_IsMovementDisabled = false;
        }
    }
}
