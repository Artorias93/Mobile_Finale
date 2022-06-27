using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitNinjaMinigame
{
    public class Spawner : MonoBehaviour
    {
        [Header("Sprites References and Spawners")]
        [SerializeField] private GameObject rg_item;
        [SerializeField] private Sprite[] rg_sprites;
        [SerializeField] private Sprite[] rg_spritesSx;
        [SerializeField] private Sprite[] rg_spritesDx;
        [SerializeField] private SpawnerBehaviour[] m_spawnPoints;
        [Header("Spawner Parameters")]
        [SerializeField] private float f_minDelay = .1f;
        [SerializeField] private float f_maxDelay = 0.75f;

        public float MinDelay
        {
            get { return f_minDelay; }
            set { f_minDelay = value; }
        }

        public float MaxDelay
        {
            get { return f_maxDelay; }
            set { f_maxDelay = value; }
        }

        private Timer m_spawnTimer;
        public static Spawner Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        // Start is called before the first frame update
        public void Start()
        {
            Spawn();
            float firstDelay = Random.Range(f_minDelay, f_maxDelay);
            m_spawnTimer = Timer.Create(firstDelay, SpawnAndChangeTime, destroyOnFinish: false, repeating: true, name: "Spawner Timer");
            m_spawnTimer.StartTimer();
        }

        //Set Spawn Time
        private void SpawnAndChangeTime()                     
        {
            Spawn();

            float delay = Random.Range(f_minDelay, f_maxDelay);
            m_spawnTimer.setDuration(delay);            
        }

        public void Spawn()
        {
            int i_sprite = Random.Range(0, rg_sprites.Length);
            int i_spawnIndex = Random.Range(0, m_spawnPoints.Length);
            m_spawnPoints[i_spawnIndex].Spawn(rg_item, rg_sprites[i_sprite], rg_spritesSx[i_sprite], rg_spritesDx[i_sprite]);
        }


        public void Stop()
        {
            m_spawnTimer.StopTimer();
        }
    }
}
