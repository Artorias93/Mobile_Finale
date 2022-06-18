using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace DarknessBumper
{
    public class Skull : MonoBehaviour, IPointerDownHandler
    {

        [HideInInspector] public float f_speedMultiplier;
        [HideInInspector] public float f_persistence;
        [HideInInspector] public int i_moveType;
        [HideInInspector] public bool b_skullDir;

        bool b_canChangeDir = true;

        float f_timeCounter = 0;
        float f_startTime;
        float f_speed;
        float f_width;
        float f_height;
        Vector3 startPos;
        Coroutine switchDirCoroutine;

        Animator anim;
        [SerializeField] Sprite explosionSprite;

        [Header("Movement Parameters")]
        [SerializeField] float f_switchDirSpeed;
        [SerializeField] float f_minSpeed;
        [SerializeField] float f_maxSpeed;
        [SerializeField] float f_minWidth;
        [SerializeField] float f_maxWidth;
        [SerializeField] float f_minHeight;
        [SerializeField] float f_maxHeight;
        [SerializeField] float f_boundHeight = 3.5f;
        [SerializeField] float f_boundWidth = 8.2f;


        private void Start()
        {
            f_speed = Random.Range(f_minSpeed, f_maxSpeed);
            f_width = Random.Range(f_minWidth, f_maxWidth);
            f_height = Random.Range(f_minHeight, f_maxHeight);

            startPos = transform.position;
            f_startTime = Time.time;
            anim = GetComponent<Animator>();
            switchDirCoroutine = StartCoroutine(SwitchDir());
        }

        void FixedUpdate()
        {
            if (DarkBumpGameManager.Instance.GameFinished) Exploding();

            if ((int)(Time.time - f_startTime) >= f_persistence && anim.GetBool("Touched") == false)
            {
                if (i_moveType != -1) DarkBumpGameManager.Instance.CountError();
                
                i_moveType = -1;
                Exploding();
            }

            float f_finalSpeed = Time.deltaTime * f_speed * f_speedMultiplier;

            if (b_canChangeDir) f_timeCounter += f_finalSpeed;
            else f_timeCounter -= f_finalSpeed;

            float f_xDeltaPos = startPos.x + Mathf.Cos(f_timeCounter) * f_width;
            float f_yDeltaPos = startPos.y + Mathf.Sin(f_timeCounter) * f_height;

            switch (i_moveType)
            {
                case 0:
                    Direction(f_xDeltaPos, f_yDeltaPos, -f_xDeltaPos, f_yDeltaPos);
                    break;
                case 1:
                    Direction(f_timeCounter, f_yDeltaPos, -f_timeCounter, f_yDeltaPos);
                    break;
                case 2:
                    Direction(f_xDeltaPos, f_timeCounter, f_xDeltaPos, -f_timeCounter);
                    break;
                case 3:
                    Direction(f_timeCounter, transform.position.y, -f_timeCounter, transform.position.y);
                    break;
                case 4:
                    Direction(transform.position.x, f_timeCounter, transform.position.x, -f_timeCounter);
                    break;
                case 5:
                    Direction(f_timeCounter, f_timeCounter, -f_timeCounter, -f_timeCounter);
                    break;
                case 6:
                    Direction(-f_timeCounter, f_timeCounter, f_timeCounter, -f_timeCounter);
                    break;
                default:
                    break;
            }

            if (Mathf.Abs(f_timeCounter) == Mathf.Abs(f_finalSpeed)) GetComponent<Renderer>().enabled = true;       //To Get Component once at the first frame

        }

        private void Direction(float f_pos1, float f_pos2, float f_pos3, float f_pos4)
        {
            if (f_pos1 <= -f_boundWidth || f_pos1 >= f_boundWidth || f_pos3 <= -f_boundWidth || f_pos3 >= f_boundWidth || f_pos2 <= -f_boundHeight || f_pos2 >= f_boundHeight || f_pos4 <= -f_boundHeight || f_pos4 >= f_boundHeight)
            {
                StopCoroutine(switchDirCoroutine);
                b_canChangeDir = !b_canChangeDir;
                switchDirCoroutine = StartCoroutine(SwitchDir());
            }

            if (!b_skullDir) transform.position = new Vector3(f_pos1, f_pos2, 1);
            else transform.position = new Vector3(f_pos3, f_pos4, 1);
        }

        private IEnumerator SwitchDir()
        {
            while (true)
            {
                yield return new WaitForSeconds(f_switchDirSpeed);
                b_canChangeDir = !b_canChangeDir;
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            anim.SetBool("Touched", true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(WaitToDestroy());
        }

        private void Exploding()
        {
            f_speed = 0;
            GetComponent<SpriteRenderer>().sprite = explosionSprite;
            anim.SetBool("Exploded", true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(WaitToDestroy());
        }

        private IEnumerator WaitToDestroy()
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
            Destroy(gameObject);
        }

    }
}
