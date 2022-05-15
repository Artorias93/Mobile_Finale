using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BalanceGame
{
    public class PlayerWeight : MonoBehaviour
    {
        public bool b_IsToScale
        {
            set { b_isToScale = value; }
        }

        // Start is called before the first frame update
        private float scaleRate = 0.2f;
        private float fastScaleRate = 0.4f;
        private float currentScaleRate;
        private float minScale, maxScale;

        private Transform parentTransform;
        private bool b_isToScale;
        private float f_contactTime = 0;

        public void InitializeGameWeight(float deltaScale)
        {
            parentTransform = transform.parent.transform;
            minScale = parentTransform.localScale.x;
            maxScale = minScale + deltaScale;
            b_isToScale = true;
            Input.gyro.enabled = true;
            currentScaleRate = scaleRate;
        }
        public void ApplyScaleRate()
        {
            Vector3 tmpScale = parentTransform.localScale;
            tmpScale += currentScaleRate * Time.deltaTime * Vector3.one;
            tmpScale.z = 0;
            parentTransform.localScale = tmpScale;
            gameObject.GetComponent<Rigidbody2D>().gravityScale += currentScaleRate * Time.deltaTime;

        }

        private float getZAngleFromGyroRotation()
        {
            Quaternion referenceRotation = Quaternion.identity;
            Quaternion deviceRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0); ;
            Quaternion eliminationOfXY = Quaternion.Inverse(
                Quaternion.FromToRotation(referenceRotation * Vector3.forward,
                                          deviceRotation * Vector3.forward)
            );
            Quaternion rotationZ = eliminationOfXY * deviceRotation;
            return rotationZ.eulerAngles.z;
        }
        public void checkScale()
        {
            //if we exceed the defined range then correct the sign of scaleRate.
            if (b_isToScale)
            {

                float roll = getZAngleFromGyroRotation();
                if (parentTransform.localScale.x >= minScale && parentTransform.localScale.x <= maxScale)
                {

                    if (180 <= roll && roll <= 360)
                    {
                        currentScaleRate = Mathf.Abs(scaleRate);
                        if (270 <= roll && roll <= 360)
                        {
                            currentScaleRate = Mathf.Abs(fastScaleRate);
                        }
                    }
                    else
                    {
                        currentScaleRate = -Mathf.Abs(scaleRate);
                        if (0 <= roll && roll <= 90)
                        {
                            currentScaleRate = -Mathf.Abs(fastScaleRate); ;
                        }
                    }
                    ApplyScaleRate();
                }
                else
                {
                    if (parentTransform.localScale.x > maxScale)
                    {
                        currentScaleRate = -Mathf.Abs(scaleRate);
                    }
                    else
                    {
                        currentScaleRate = Mathf.Abs(scaleRate); ;
                    }
                    ApplyScaleRate();
                }
            }
        }

        private void Update()
        {
            checkScale();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("WinTrigger"))
            {
                f_contactTime += Time.deltaTime;
                Debug.LogWarning("Win time: " + f_contactTime);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.LogWarning("Win time: " + f_contactTime);
        }
    }
}