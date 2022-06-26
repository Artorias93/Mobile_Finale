using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMadCthulhuMinigame
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField]
        private Transform firepoint;
        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private Animator animator;
        
        float timebetween;
        [SerializeField]
        private float starttimebetween;
        
        void Start()
        {
            timebetween = starttimebetween;
        }

        public int direction()
        {
            if (firepoint.localPosition.x < 0f)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        void Update()
        {
            if (TheMadGameManager.Instance.GameFinished)
            {
                animator.enabled = false;
                return;
            }
            if (timebetween <= 0)
            {
                GameObject newBullet = Instantiate(bullet, firepoint.position, firepoint.rotation);
                newBullet.transform.SetParent(this.transform);
                

                newBullet.GetComponent<Bullet>().Direction = direction();
                
                timebetween = starttimebetween;
            }
            else
            {
                timebetween -= Time.deltaTime;
            }
        }
        

    }
}

