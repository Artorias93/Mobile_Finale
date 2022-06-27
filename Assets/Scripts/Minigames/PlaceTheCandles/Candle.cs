using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaceTheCandles
{
    public class Candle : MonoBehaviour
    {
        Animator anim;
        bool b_triggerCollision = false;
        bool b_collName = false;
        bool b_canMove = true;
        bool b_destroy = false;
        GameObject obj_collider;


        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        }

        // Update is called once per frame
        void Update()
        {
            if (PlaceCandlesGameManager.Instance.GameFinished && anim != null) Destroy(anim);
            if (PlaceCandlesGameManager.Instance.CandleCheck && !b_destroy)
            {
                b_destroy = true;
                anim.SetTrigger("Destroy");
            }
        }

        void OnMouseDrag()
        {
            if (b_canMove && Time.timeScale==1)
            {
                Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                transform.position = objPosition + (Vector3.forward * 3);
            }
        }

        private void OnMouseUp()
        {
            if (b_triggerCollision && b_collName)
            {
                b_canMove = false;
                transform.position = obj_collider.transform.position + new Vector3(0, 0.7f, 0);
                transform.SetParent(obj_collider.transform.parent);
                GetComponent<Collider2D>().enabled = false;
                Destroy(obj_collider);
                PlaceCandlesGameManager.Instance.CountCandles();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            b_triggerCollision = true;
            b_collName = collision.gameObject.name.StartsWith("Place");
            obj_collider = collision.gameObject;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            b_triggerCollision = false;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            b_triggerCollision = true;
            b_collName = collision.gameObject.name.StartsWith("Place");
            obj_collider = collision.gameObject;
        }
    }
}
