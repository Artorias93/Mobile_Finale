using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaceTheCandles
{
    public class Candle : MonoBehaviour
    {
        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Animator>().SetFloat("Speed", Random.Range(0.8f, 1.2f));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
