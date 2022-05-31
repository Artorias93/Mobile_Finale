using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheMadCthulhuMinigame
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField]
        private float speed, distance;

        [SerializeField]
        private bool isMovingVertically;

        private bool movingRight = true;
        [SerializeField]
        private Transform groundDetector;

        void Update()
        {
           
            Move();
        }

        private void Move()
        {
            RaycastHit2D groundInfo;
            if (isMovingVertically)
            {
                groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.left, distance);
                
                transform.Translate(Vector2.down * speed * Time.deltaTime);
                
            }
            else
            {
                groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, distance);
                //if (groundInfo.collider.tag == "PatrolZone")

                    transform.Translate(Vector2.right * speed * Time.deltaTime);
               
            }

            
            if (groundInfo.collider == false /*groundInfo.collider.tag != "PatrolZone"*/)
            {
                if (movingRight == true)
                {
                    if (!isMovingVertically)
                    {

                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingRight = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(-180, 0, 0);
                        movingRight = false;
                    }


                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
           
            }
        }

        
    }
}