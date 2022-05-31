using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
   
        private float dirY;
        private float speed = 8f;
        private bool isLadder;
        private bool isClimbing;

        [SerializeField] private Rigidbody2D rb;

        void Update()
        {

            if (isLadder && Mathf.Abs(dirY) > 0f)
            {
                isClimbing = true;
            }
        }

        private void FixedUpdate()
        {
            if (isClimbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, dirY * speed);
            }
            else
            {
                rb.gravityScale = 4f;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isLadder = false;
                isClimbing = false;
            }
        }
    public void MoveUp()
    {

        dirY = 1;
    }
    public void MoveDown()
    {

        dirY = -1;
    }
    public void StopMoving()
    {
        dirY = 0;
        rb.velocity = Vector2.zero;
    }
}

