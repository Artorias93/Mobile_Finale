using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TheMadCthulhuMinigame
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private bool b_goingToB=true;
        
        //[SerializeField]
        //private Transform groundDetector;
        [SerializeField]
        private GameObject m_pointA, m_pointB;

        private void Start()
        {
            transform.position = m_pointA.transform.position;
        }
        void Update()
        {
           
        }
        private void FixedUpdate()
        {
            if (b_goingToB) 
            {
                MoveToB();
                if(transform.position==m_pointB.transform.position)
                b_goingToB = false;
            }
            if (!b_goingToB)
            {
                MoveToA();
                if (transform.position == m_pointA.transform.position)
                    b_goingToB = true;
            }
          
            
        }

        private void MoveToA()
        {
           
                transform.position = Vector3.MoveTowards(transform.position, m_pointA.transform.position, speed);
         
         
        }
        private void MoveToB()
        {
                transform.position = Vector3.MoveTowards(transform.position, m_pointB.transform.position, speed);

        }

        
    }
}