using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] rg_items;
    public Transform[] rg_spawnPoints;
    public float f_minDelay = .1f;
    public float f_maxDelay = 0.75f;
    private float f_minSpawnForceX;
    private float f_maxSpawnForceX;
    private float f_minSpawnForceY = 10;
    private float f_maxSpawnForceY = 15;
    private float f_randomForceX;
    private float f_randomForceY;
    public int i_torqueRotationItem;
    private int i_item;
    private int i_spawnIndex;
    //private int i_minSpawnOffsetX;
    //private int i_maxSpawnOffsetX;


    private IEnumerator countdown;
    //public float f_timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Timer>().Initialize(5);
        //Timer.OnTimeEnd += MethodToCall();
        
        countdown = Countdown(Random.Range(f_minDelay, f_maxDelay));
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
            float delay = Random.Range(f_minDelay, f_maxDelay);
            yield return new WaitForSeconds(delay);
            Spawn();
        }
    }

    public void Spawn()
    {
        i_spawnIndex = Random.Range(0, rg_spawnPoints.Length);
        Transform spawnPoint = rg_spawnPoints[i_spawnIndex];
        i_item = Random.Range(0, 4);
        GameObject obj_item = Instantiate(rg_items[i_item], spawnPoint.position, spawnPoint.rotation);

        switch (i_spawnIndex)
        {
            case 0:
                f_minSpawnForceX = 0;
                f_maxSpawnForceX = 6;
                break;
            case 1:
                f_minSpawnForceX = -0.5f;
                f_maxSpawnForceX = 5;
                break;
            case 2:
                f_minSpawnForceX = -1f;
                f_maxSpawnForceX = 4;
                break;
            case 3:
                f_minSpawnForceX = -2f;
                f_maxSpawnForceX = 3.5f;
                break;
            case 4:
                f_minSpawnForceX = -3;
                f_maxSpawnForceX = 3;
                break;
            case 5:
                f_minSpawnForceX = 2;
                f_maxSpawnForceX = -3.5f;
                break;
            case 6:
                f_minSpawnForceX = 1f;
                f_maxSpawnForceX = -4;
                break;
            case 7:
                f_minSpawnForceX = 0.5f;
                f_maxSpawnForceX = -5;
                break;
            default:
                f_minSpawnForceX = 0;
                f_maxSpawnForceX = -6;
                break;
        }

        f_randomForceX = Random.Range(f_minSpawnForceX, f_maxSpawnForceX);
        f_randomForceY = Random.Range(f_minSpawnForceY, f_maxSpawnForceY);
        
        obj_item.GetComponent<Rigidbody2D>().AddForce(new Vector2 (f_randomForceX, f_randomForceY),ForceMode2D.Impulse);
        if (f_randomForceX < 0) obj_item.GetComponent<Rigidbody2D>().AddTorque(i_torqueRotationItem);
        else obj_item.GetComponent<Rigidbody2D>().AddTorque(-i_torqueRotationItem);

        Destroy(obj_item, 3f);

        //Debug.Log(i_item);
    }

}
