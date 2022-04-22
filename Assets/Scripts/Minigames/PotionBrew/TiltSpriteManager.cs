using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBrewMinigame
{
    public class TiltSpriteManager : MonoBehaviour
    {

        [SerializeField]
        private Sprite s_potionNoFlow;
        [SerializeField]
        private Sprite s_potionSlowFlow;
        [SerializeField]
        private Sprite s_potionMediumFlow;
        [SerializeField]
        private Sprite s_potionFastFlow;
        [SerializeField]
        private Sprite s_potionOverFlow;

        private TiltController m_tiltController;
        private SpriteRenderer m_spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            m_tiltController = transform.GetChild(0).GetComponent<TiltController>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_spriteRenderer.sprite = s_potionNoFlow;

        }

        // Update is called once per frame
        void Update()
        {
            if (m_tiltController.e_FlowState.Equals(TiltController.FLOW_SPEED.NO)) {
                m_spriteRenderer.sprite = s_potionNoFlow;
            }else if (m_tiltController.e_FlowState.Equals(TiltController.FLOW_SPEED.SLOW)) {
                m_spriteRenderer.sprite = s_potionSlowFlow;
            }else if (m_tiltController.e_FlowState.Equals(TiltController.FLOW_SPEED.MEDIUM))
            {
                m_spriteRenderer.sprite = s_potionMediumFlow;
            }
            else if (m_tiltController.e_FlowState.Equals(TiltController.FLOW_SPEED.FAST))
            {
                m_spriteRenderer.sprite = s_potionFastFlow;
            }
            else if (m_tiltController.e_FlowState.Equals(TiltController.FLOW_SPEED.OVERFLOW))
            {
                m_spriteRenderer.sprite = s_potionOverFlow;
            }
        }
    }
}