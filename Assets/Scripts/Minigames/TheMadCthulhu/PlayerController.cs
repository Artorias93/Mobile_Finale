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
        private Rigidbody2D rigidbody;
        [SerializeField]
        private float f_repellingForce=10000;
        public float Lives
        {
            get { return lives; }
            set { lives = (int)value; }
        }

        // Start is called before the first frame update
        void Start()
        {
          
                lives = gameManager.SelectedParameters.i_Lives;

        }
       
        // Update is called once per frame
        void Update()
        {

            if (lives <= 0)
                gameManager.LoseMinigame();
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Finish")
                gameManager.WinMinigame();
           
            if (collision.gameObject.tag == "Enemy")
            {
                lives--;
                rigidbody.AddForce((Vector3.up - collision.transform.position) * f_repellingForce);
            }

            
        }
    }
}
