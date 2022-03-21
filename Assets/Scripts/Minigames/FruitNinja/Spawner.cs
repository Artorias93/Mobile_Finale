using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] rg_items;
    private int i_item;

    private IEnumerator countdown;
    float f_timeLeft = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Timer>().Initialize(5);
        //Timer.OnTimeEnd += MethodToCall();
        countdown = Countdown(f_timeLeft);
        StartCoroutine(countdown);

    }

    // Update is called once per frame

    void Update()
    {

    }

    private IEnumerator Countdown(float f_timeLeft)                               
    {
        while (true)
        {                     
            yield return new WaitForSeconds(f_timeLeft);
            Spawn();
        }
    }

    public void Spawn()
    {
        i_item = Random.Range(0, 4);
        Vector3 v3_spawn_position  = new Vector3(Random.Range(-6, 6), -7, 0);
        GameObject obj_item = Instantiate(rg_items[i_item], v3_spawn_position, transform.rotation);
        obj_item.GetComponent<Rigidbody2D>().AddForce(new Vector2 (Random.Range(-100, 100), Random.Range(500, 700)));

        Debug.Log(i_item);
    }


}
