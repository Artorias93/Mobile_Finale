using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BalanceGame
{
    public class GameWeight : MonoBehaviour
    {
        public bool b_IsToScale
        {
            set { b_isToScale = value; }
        }

        // Start is called before the first frame update
        private float scaleRate = 0.2f;
        private float minScale, maxScale;

        private Transform parentTransform;
        private bool b_isToScale;
        private float f_contactTime = 0;
        private float minTimeStopRescale = 1;
        private float maxTimeStopRescale = 2;

        public void InitializeGameWeight(float deltaScale)
        {
            parentTransform = transform.parent.transform;
            minScale = parentTransform.localScale.x;
            maxScale = minScale + deltaScale;
            b_isToScale = true;
        }
        public void ApplyScaleRate()
        {
            Vector3 tmpScale = parentTransform.localScale;
            tmpScale += scaleRate * Time.deltaTime * Vector3.one;
            tmpScale.z = 0;
            parentTransform.localScale = tmpScale;
            gameObject.GetComponent<Rigidbody2D>().gravityScale += scaleRate * Time.deltaTime;

        }

        private IEnumerator stopAndStartScale()
        {
            b_isToScale = false;
            yield return new WaitForSeconds(Random.Range(minTimeStopRescale, maxTimeStopRescale));
            b_isToScale = true;
        }

        public void checkScale()
        {
            //if we exceed the defined range then correct the sign of scaleRate.
            if (b_isToScale)
            {
                if (parentTransform.localScale.x < minScale)
                {
                    scaleRate = Mathf.Abs(scaleRate);
                }
                else if (parentTransform.localScale.x > maxScale)
                {
                    scaleRate = -Mathf.Abs(scaleRate);
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
                Debug.LogWarning("Lose time: " + f_contactTime);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.LogWarning("Lose time: " + f_contactTime);
        }
    }
}