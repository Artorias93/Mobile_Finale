using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeDungeonMinigame
{
    public class EscapeMinigameManager : AMinigameManager
    {
        [Header("Game Settings")]
        [SerializeField] private float frameTime = 0.5f;

        public static EscapeMinigameManager Instance;

        public delegate void UpdateEvent();
        public static event UpdateEvent OnUpdate;

        private float f_elapsedTime;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);


        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            f_elapsedTime += Time.deltaTime;
            if(f_elapsedTime > frameTime)
            {
                f_elapsedTime -= frameTime;
                if(OnUpdate != null)
                    OnUpdate();
            }

        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            throw new System.NotImplementedException();
        }

        protected override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }
    }
}

