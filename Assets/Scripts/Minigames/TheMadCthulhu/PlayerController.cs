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
        [SerializeField]
        private float f_repellingForce=2;
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
                //rigidbody.AddForceAtPosition((Vector3.zero - collision.transform.position) * f_repellingForce, collision.transform.position);
                Vector3 forceDirection = (collision.transform.position-this.transform.position).normalized;
                Debug.DrawRay(this.transform.position, this.transform.position + forceDirection, Color.red);
                
                
                m_rigidbody.AddForce(forceDirection * f_repellingForce, ForceMode2D.Impulse);
            }


        }
    }
}
