using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BalanceGame
{
    public class PlayerWeight : MonoBehaviour
    {

        public float f_ContactTime
        {
            get { return f_contactTime; }
        }

        [SerializeField]
        private int i_StartPositiveScaleAngle;
        [SerializeField]
        private int i_StartNegativeScaleAngle;
        [SerializeField]
        private int i_DeltaFastScaleAngle;


        // Start is called before the first frame update
        private float scaleRate;
        private float fastScaleRate;
        private float currentScaleRate;
        private float minScale, maxScale,minGrav,maxGrav;

        private Transform parentTransform;
        private bool b_isToScale;
        private float f_contactTime = 0;
        private Rigidbody2D rigidBody;


        public void InitializeGameWeight(float deltaScale, float f_initialGravityScale, float f_rescaleRate)
        {
            parentTransform = transform.parent.transform;
            scaleRate = f_rescaleRate;
            fastScaleRate = scaleRate * 2;
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = f_initialGravityScale;
            minGrav = rigidBody.gravityScale;
            maxGrav = minGrav + deltaScale;
            minScale = parentTransform.localScale.x;
            maxScale = minScale + deltaScale;
            b_isToScale = true;
            Input.gyro.enabled = true;
            currentScaleRate = 0;
        }
        public void ApplyScaleRate()
        {
            Vector3 tmpScale = parentTransform.localScale;
            tmpScale += currentScaleRate * Time.deltaTime * Vector3.one;
            float tmpGrav = rigidBody.gravityScale;
            tmpGrav += currentScaleRate * Time.deltaTime;
            if (tmpScale.x > maxScale) {
                tmpScale.x = maxScale;
                tmpScale.y = maxScale;
                tmpGrav = maxGrav;
            } else if (tmpScale.x < minScale) {
                tmpScale.x = minScale;
                tmpScale.y = minScale;
                tmpGrav = minGrav;
            }
            tmpScale.z = 0;
            parentTransform.localScale = tmpScale;
            rigidBody.gravityScale = tmpGrav;
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
                    Debug.LogWarning(roll);
                    if (i_StartPositiveScaleAngle <= roll && roll <= 360)
                    {
                        currentScaleRate = Mathf.Abs(scaleRate);
                        if (i_StartPositiveScaleAngle + i_DeltaFastScaleAngle <= roll && roll <= 360)
                        {
                            currentScaleRate = Mathf.Abs(fastScaleRate);
                        }
                    }
                    else if (i_StartNegativeScaleAngle <= roll && roll <= 180)
                    {
                        currentScaleRate = -Mathf.Abs(scaleRate);
                        if (i_StartNegativeScaleAngle +i_DeltaFastScaleAngle <= roll && roll <= 180)
                        {
                            currentScaleRate = -Mathf.Abs(fastScaleRate); ;
                        }
                    }
                    else {
                        currentScaleRate = 0;
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
            }
        }
    }
}