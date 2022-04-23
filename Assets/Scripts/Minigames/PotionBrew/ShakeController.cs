using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBrewMinigame
{
    public class ShakeController : MonoBehaviour
    {   [SerializeField]
        private float f_ShakeTreshOld = 3.5f;
        [SerializeField]
        private Sprite s_baseSprite;
        [SerializeField]
        private Sprite s_overFlowSprite;

        private Rigidbody2D m_rigidBody;
        private SpriteRenderer spriteRenderer;
        private float f_magnitude = 0f;
        private Vector3 m_acceleration = Vector3.zero;
        //Public for debug
        public bool b_startShake = false;
        private int i_shakesCount=0;
        public int i_ShakeCount {
            get { return i_shakesCount; }
        }

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        }
        public void InitShakeController()
        {
            b_startShake = true;
        }
        // Update is called once per frame
        void Update()
        {
            f_magnitude = Input.acceleration.magnitude;
            m_acceleration = Input.acceleration;
        }
        private void FixedUpdate()
        {
            if (b_startShake && f_magnitude >= f_ShakeTreshOld)
            {
                i_shakesCount++;
                shake(m_acceleration);
            }

        }

        private void shake(Vector3 deviceAcceleration)
        {
            deviceAcceleration.y = Mathf.Pow(deviceAcceleration.z, 2);
            m_rigidBody.AddForce(deviceAcceleration, ForceMode2D.Impulse);
        }
        public void ResetShakeCount() {
            i_shakesCount = 0;
        }
        public void setBaseSprite() {
            if (!spriteRenderer.sprite.Equals(s_baseSprite)) {
                spriteRenderer.sprite = s_baseSprite;
            }
        }
        public void setOverFlowSprite()
        {
            if (!spriteRenderer.sprite.Equals(s_overFlowSprite))
            {
                spriteRenderer.sprite = s_overFlowSprite;
            }
        }
    }
}