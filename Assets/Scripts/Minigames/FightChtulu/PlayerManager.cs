using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FightChtulu
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite s_emptySpace;
        [SerializeField]
        private Sprite s_playerSprite;

        public static PlayerManager Instance;
        private List<GameObject> m_positions= new List<GameObject>();
        private GameObject playerCurrentPosition=null;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitializePlayer();
        }

        public void InitializePlayer() {
            foreach (Transform child in transform)
            {
                m_positions.Add(child.gameObject);
            }

            GameObject startPosition = m_positions[Random.Range(0, m_positions.Count)];
            foreach (GameObject position in m_positions)
            {
                position.GetComponent<SpriteRenderer>().sprite = s_emptySpace;
            }
            setCurrentPosition(startPosition,false);
        }


        private void setCurrentPosition(GameObject position, bool resetCurrent) {
            if (resetCurrent) {
                playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = s_emptySpace;
                playerCurrentPosition.GetComponent<Collider2D>().enabled = true;
            }
            playerCurrentPosition = position;
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = s_playerSprite;
            playerCurrentPosition.GetComponent<Collider2D>().enabled = false;
        }
        public void setPlayerPosition(GameObject obj)
        {
            if (m_positions.Contains(obj) && !playerCurrentPosition.Equals(obj)) {
                Vector3 dir = playerCurrentPosition.transform.position - obj.transform.position;
                //If movement is straight (not diagonal)
                if (dir.x == 0 || dir.y == 0)
                {
                    //Length of the ray
                    float rayLength = 100f;
                    dir *= -1;

                    //Get the first object hit by the ray
                    RaycastHit2D hit = Physics2D.Raycast(playerCurrentPosition.transform.position, dir, rayLength);

                    if (hit.collider == obj.GetComponent<Collider2D>())
                    {
                        setCurrentPosition(obj,true);
                    }

                    //Debug.DrawRay(playerCurrentPosition.transform.position, dir * rayLength, Color.red);

                }
                else {
                    Debug.LogWarning("Can't move diagonal");
                }

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
