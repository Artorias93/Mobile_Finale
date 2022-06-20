using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMadCthulhuMinigame
{
    public class Bullet : MonoBehaviour
    {
        
        [SerializeField]
        private float speed, timeToDestroy = 1;
        [SerializeField]
        private Rigidbody2D rb;

        private int i_direction;//1=dx, -1=sx

        public int Direction
        {
            set
            {
                i_direction = value;

                rb.velocity = new Vector2(speed * i_direction, 0f);
                Destroy(gameObject, timeToDestroy);
            }
        }


        void Start()
        {

        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag=="Player")
            Destroy(gameObject);
        }
    }
}