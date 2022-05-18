using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BalanceGame
{
    public class GameWeight : MonoBehaviour
    {
        public float f_ContactTime
        {
            get { return f_contactTime; }
        }
        // Start is called before the first frame update
        private float scaleRate;
        private float minScale, maxScale;

        private Transform parentTransform;
        private bool b_isToScale;
        private float f_contactTime = 0;
        private float stopRescaleSeconds;
        private Rigidbody2D rigidBody;
        private Animator animator;

        public void InitializeGameWeight(float deltaScale, float f_initialGravityScale, float f_stopRescaleSeconds, float f_rescaleRate)
        {

            parentTransform = transform.parent.transform;
            scaleRate = f_rescaleRate;
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            rigidBody.gravityScale = f_initialGravityScale;
            minScale = parentTransform.localScale.x;
            maxScale = minScale + deltaScale;
            stopRescaleSeconds = f_stopRescaleSeconds;
            b_isToScale = true;
            setBiggerAnimation();
        }
        public void ApplyScaleRate()
        {
            Vector3 tmpScale = parentTransform.localScale;
            tmpScale += scaleRate * Time.deltaTime * Vector3.one;
            tmpScale.z = 0;
            parentTransform.localScale = tmpScale;
            rigidBody.gravityScale += scaleRate * Time.deltaTime;

        }

        private IEnumerator stopAndStartScale()
        {
            b_isToScale = false;
            setIdleAnimation();
            yield return new WaitForSeconds(stopRescaleSeconds);
            if (scaleRate > 0) {
                setBiggerAnimation();
            } else {
                setSmallerAnimation();
            }
            b_isToScale = true;
        }

        private void setBiggerAnimation()
        {
            animator.SetBool("idle", false);
            animator.SetBool("small", false);
            animator.SetBool("big", true);
        }

        private void setSmallerAnimation()
        {
            animator.SetBool("idle", false);
            animator.SetBool("small", true);
            animator.SetBool("big", false);
        }

        private void setIdleAnimation() {
            animator.SetBool("idle", true);
            animator.SetBool("small", false);
            animator.SetBool("big", false);
        }
        public void checkScale()
        {
            //if we exceed the defined range then correct the sign of scaleRate.
            if (b_isToScale)
            {
                if (parentTransform.localScale.x < minScale)
                {
                    scaleRate = Mathf.Abs(scaleRate);
                    StartCoroutine(stopAndStartScale());
                }
                else if (parentTransform.localScale.x > maxScale)
                {
                    scaleRate = -Mathf.Abs(scaleRate);
                    StartCoroutine(stopAndStartScale());
                }
                ApplyScaleRate();
            }
        }

        private void Update()
        {
            checkScale();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("LoseTrigger"))
            {
                f_contactTime += Time.deltaTime;
            }
        }
    }
}