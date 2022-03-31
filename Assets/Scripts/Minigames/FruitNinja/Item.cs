using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitNinjaMinigame
{
    public class Item : MonoBehaviour
    {
        public GameObject obj_fruitSlicedPrefab;
        public int i_status = 0;

        void OnTriggerEnter2D(Collider2D col)
        {

            if (col.CompareTag("Blade"))
            {
                //Splitting Sliced Objects
                GameObject obj_slicedFruit = Instantiate(obj_fruitSlicedPrefab, transform.position, transform.rotation);
                obj_slicedFruit.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,5),ForceMode2D.Impulse);
                obj_slicedFruit.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 5), ForceMode2D.Impulse);

                Destroy(obj_slicedFruit, 3f);
                Destroy(gameObject);
            }

            //Add 1 to Not-Sliced Objects
            if (i_status == 1 && col.CompareTag("OutDown")) 
                GameManager.Instance.increaseErrorCount();

        }

        void OnTriggerExit2D(Collider2D col)
        {
            //Set New Object "Coming In-Screen"
            if (col.CompareTag("OutDown")) if (i_status == 0) i_status++;
        }

    }
}
