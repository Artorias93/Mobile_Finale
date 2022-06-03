using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightCthulhu
{
    public class DefendableItem : MonoBehaviour
    {
        private Transform startTrasform;
        public bool b_isGrabbed = false;
        void Start()
        {
            startTrasform= transform.parent.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!transform.position.Equals(startTrasform.position) && !b_isGrabbed) {
                transform.position = startTrasform.position;                
            }
        }
    }
}