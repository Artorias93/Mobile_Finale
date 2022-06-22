using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheMadCthulhuMinigame
{
    public class LadderMovement : MonoBehaviour
    {

        private float f_dirY;
        [SerializeField]
        private float f_speed = 100f;
        [SerializeField]
        private float f_gravityScale = 10f;
        private bool b_isLadder;
        private bool b_isClimbing;

        [SerializeField] private Rigidbody2D rb;

        void Update()
        {
            
            if (b_isLadder && Mathf.Abs(f_dirY) != 0f)
            {
                b_isClimbing = true;
            }
        }

        private void FixedUpdate()
        {
            if (b_isClimbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, f_dirY * f_speed);
                
            }
            else
            {
                rb.gravityScale = f_gravityScale;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                b_isLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                b_isLadder = false;
                b_isClimbing = false;
            }
        }
        public void MoveUp()
        {

            f_dirY = 1;
        }
        public void MoveDown()
        {

            f_dirY = -1;
        }
        public void StopMoving()
        {
            f_dirY = 0;
            rb.velocity = Vector2.zero;
        }
    }

}