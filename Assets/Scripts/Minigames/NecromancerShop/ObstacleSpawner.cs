using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NecromancerShop;

namespace NecromancerShop
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_target;
        [SerializeField]
        private GameObject m_obstaclePrefab;
        [SerializeField]
        private float f_speed = 2.5f;
        [SerializeField]
        private NecromancerShopGM gameManager;

        public bool b_isInitialized {
            get { return b_initialized; }
        }

        private bool b_canMove = false, b_initialized = false;
        private GameObject m_obstacle;
        private RectTransform m_obstacleRect;
        private RectTransform m_spawnerRect;
        private GameObject m_currentTarget;
        private int i_minSecondsStop;
        private int i_maxSecondsStop;


        // Start is called before the first frame update
        void Start()
        {
            
            m_spawnerRect = GetComponent<RectTransform>();
            m_obstacle = Instantiate(m_obstaclePrefab, m_spawnerRect.transform);
            m_obstacleRect = m_obstacle.GetComponent<RectTransform>();
            m_currentTarget = m_target;
        }

        public void InitializeSpawner(int i_minSeconds, int i_maxSeconds) {
            i_minSecondsStop = i_minSeconds;
            i_maxSecondsStop = i_maxSeconds;
            b_initialized = true;
            b_canMove = true;
            gameManager.i_inizializedSpawners++;
        }
        public void StopSpawner()
        {
            b_initialized = false;
            b_canMove = false;
            gameManager.i_inizializedSpawners--;
        }
        // Update is called once per frame
        void Update()
        {
            if (b_canMove && b_initialized)
            {

                if (m_obstacleRect.transform.position == m_currentTarget.GetComponent<RectTransform>().position) {
                    if (m_obstacleRect.position == m_spawnerRect.position)
                    {
                        m_currentTarget = m_target;
                        StopSpawner();
                    }
                    else {
                        m_currentTarget = gameObject;
                        StartCoroutine(stopAndStartMovement(i_minSecondsStop, i_maxSecondsStop));
                    }

                }
               m_obstacleRect.position = Vector3.MoveTowards(m_obstacleRect.position, m_currentTarget.GetComponent<RectTransform>().position, f_speed * Time.deltaTime);
            }
        }

        private IEnumerator stopAndStartMovement(float i_minSeconds,float i_maxSeconds) {
            b_canMove = false;
            yield return new WaitForSeconds(Random.Range(i_minSeconds, i_maxSeconds));
            b_canMove = true;
        }
    }
}