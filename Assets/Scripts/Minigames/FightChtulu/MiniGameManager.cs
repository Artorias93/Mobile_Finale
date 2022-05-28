using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightCthulhu
{
    public class MiniGameManager : AMinigameManager
    {

        [SerializeField] 
        private float frameTime = 0.5f;

        public delegate void UpdateEvent();
        public static event UpdateEvent OnUpdate;

        private float f_elapsedTime=0;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            f_elapsedTime += Time.deltaTime;
            if (f_elapsedTime > frameTime)
            {
                f_elapsedTime -= frameTime;
                if (OnUpdate != null)
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