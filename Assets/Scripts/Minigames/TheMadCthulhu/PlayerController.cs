using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMadCthulhuMinigame
{
    public class PlayerController : MonoBehaviour
    {
        private int lives;
        [SerializeField]
        private TheMadGameManager gameManager;
        [SerializeField]
        private Rigidbody2D m_rigidbody;
        public float Lives
        {
            get { return lives; }
            set { lives = (int)value; }
        }

        public void InitializePlayerLives(int playerLives) {
            lives = playerLives;
        }
        // Update is called once per frame
        void Update()
        {

            if (lives <= 0 && gameManager.b_GameStarted)
                gameManager.LoseMinigame();
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Finish")
                gameManager.WinMinigame();
           
            if (collision.gameObject.tag == "Enemy")
            {
                lives--;
            }


        }
    }
}
