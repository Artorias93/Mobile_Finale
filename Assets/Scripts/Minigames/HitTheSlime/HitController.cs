using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

namespace SlimeMinigame
{
    public class HitController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Animation Parameters")]
        [SerializeField] private Animator m_animator;
        [SerializeField, Tooltip("Name of parameters to control hit animation in animator")] 
        private string s_HitParameterName = "Hit";

        private bool b_touching = false;
        private int i_hits = 0;

        public bool Touching
        {
            get { return b_touching; }
        }
        public int Hits
        {
            get { return i_hits; }
        }

        private void Awake()
        {
            if(m_animator == null)
            {
                m_animator = GetComponent<Animator>();
            }
            Assert.IsNotNull(m_animator, "Can't find animator");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            b_touching = true;
            m_animator?.SetBool(s_HitParameterName, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            b_touching = false;
            m_animator?.SetBool(s_HitParameterName, false);
            i_hits++;
            Debug.Log("Hits: " + i_hits);
        }

    }
}
