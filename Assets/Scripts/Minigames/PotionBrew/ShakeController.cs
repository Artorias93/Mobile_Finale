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
        [SerializeField]
        private float f_shakeSpeed=0.5f;

        private Rigidbody2D m_rigidBody;
        private SpriteRenderer spriteRenderer;
        private float f_magnitude = 0f;
        private Vector3 m_acceleration = Vector3.zero;
        //Public for debug
        public bool b_startShake = false;
        private int i_shakesCount=0;
        private Vector2 startingPosition;
        public int i_ShakeCount {
            get { return i_shakesCount; }
        }

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
            startingPosition = transform.position;
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
            if (b_startShake && f_magnitude >= f_ShakeTreshOld)
            {
                i_shakesCount++;
                shake();
            }
            else if (!transform.position.Equals(startingPosition)) {
                transform.position = startingPosition;
            }
        }

        private void shake()
        {
            Vector2 tmpOriginalPos = transform.position;
            tmpOriginalPos.y = Mathf.Sin(Time.deltaTime * f_shakeSpeed) * 0.5f;
            transform.position = tmpOriginalPos;
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