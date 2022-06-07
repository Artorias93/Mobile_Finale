using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightCthulhu
{
    public class DefendableItem : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> idleSprites;
        [SerializeField]
        private List<Sprite> grabbedSprites;

        private int i_spriteIndex = 0;
        private Transform startTrasform;
        public bool b_isGrabbed = false;
        void Start()
        {
            startTrasform= transform.parent.transform;
            MiniGameManager.OnUpdate += UpdateInTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (!transform.position.Equals(startTrasform.position) && !b_isGrabbed) {
                transform.position = startTrasform.position;
                
            }
        }

        void UpdateInTime() {
            changeSpriteIndex();
            if (b_isGrabbed)
            {
                GetComponent<SpriteRenderer>().sprite = grabbedSprites[i_spriteIndex];
            }
            else {
                GetComponent<SpriteRenderer>().sprite = idleSprites[i_spriteIndex];
            }
        }

        private void changeSpriteIndex()
        {
            if (i_spriteIndex == idleSprites.Count - 1)
            {
                i_spriteIndex = 0;
            }
            else
            {
                i_spriteIndex++;
            }

        }
        private void OnDestroy()
        {
            MiniGameManager.OnUpdate -= UpdateInTime;
        }
    }
}