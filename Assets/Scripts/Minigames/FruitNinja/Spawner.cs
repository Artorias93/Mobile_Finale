using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitNinjaMinigame
{
    public class Spawner : MonoBehaviour
    {
        [Header("Sprites References and Spawners")]
        [SerializeField] private GameObject[] rg_items;
        [SerializeField] private Transform[] rg_spawnPoints;
        [Header("Spawner Parameters")]
        [SerializeField] private float f_minDelay = .1f;
        [SerializeField] private float f_maxDelay = 0.75f;
        [SerializeField] private float f_minSpawnForceX;
        [SerializeField] private float f_maxSpawnForceX;
        [SerializeField] private float f_minSpawnForceY = 11;
        [SerializeField] private float f_maxSpawnForceY = 15;
        [SerializeField] private float f_randomForceX;
        [SerializeField] private float f_randomForceY;
        [SerializeField] private int i_torqueRotationItem;
        [SerializeField] private int i_item;
        [SerializeField] private int i_spawnIndex;

        // Start is called before the first frame update
        void Start()
        {
            float firstDelay = Random.Range(f_minDelay, f_maxDelay);
            StartCoroutine(Countdown(firstDelay));
        }

        // Update is called once per frame

        void Update()
        {

        }

        //Set Spawn Time
        private IEnumerator Countdown(float f_timeLeft)                     
        {
            while (!GameManager.Instance.GameFinished)
            {
                float delay = Random.Range(f_minDelay, f_maxDelay);
                yield return new WaitForSeconds(delay);
                Spawn();
            }
        }

        //Choosing Spawn Points, Setting Min/Max Force to GameObjects and Instancing 
        public void Spawn()
        {
            i_spawnIndex = Random.Range(0, rg_spawnPoints.Length);
            Transform spawnPoint = rg_spawnPoints[i_spawnIndex];
            i_item = Random.Range(0, rg_items.Length);
            GameObject obj_item = Instantiate(rg_items[i_item], spawnPoint.position, spawnPoint.rotation);


            //TODO: Divide in subscripts to delegate behaviour
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

            obj_item.GetComponent<Rigidbody2D>().AddForce(new Vector2(f_randomForceX, f_randomForceY), ForceMode2D.Impulse);
            if (f_randomForceX < 0) obj_item.GetComponent<Rigidbody2D>().AddTorque(i_torqueRotationItem);
            else obj_item.GetComponent<Rigidbody2D>().AddTorque(-i_torqueRotationItem);

            Destroy(obj_item, 3f);
        }

    }
}
