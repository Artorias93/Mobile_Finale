using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace FruitNinjaMinigame
{
    public class SpawnerBehaviour : MonoBehaviour
    {
        [Header("Spawner parameters")]
        [SerializeField] private GameObject obj_direction;
        [SerializeField, Min(0.0f)] private float f_minForce = 0.1f, f_maxForce = 5.0f;
        [SerializeField] private float f_torque = 30f;
        [SerializeField] private float f_itemLifetime = 10f;

        private Vector3 v3_cachedDirection;

        private void Awake()
        {
            if (obj_direction == null)
                obj_direction = transform.GetChild(0).gameObject;
            Assert.IsNotNull(obj_direction, "Can't find direction object");

            v3_cachedDirection = obj_direction.transform.position - transform.position;
            v3_cachedDirection.Normalize();
        }

        public void Spawn(GameObject objectToSpawn)
        {
            float randomMagnitude = Random.Range(f_minForce, f_maxForce);
            float randomTorque = Random.Range(-f_torque, f_torque);

            GameObject spawnedItem = Instantiate(objectToSpawn, transform.position, Quaternion.identity, null);
            Rigidbody2D rbody = spawnedItem.GetComponent<Rigidbody2D>();

            if (rbody != null)
            {
                rbody.AddForce(randomMagnitude * v3_cachedDirection, ForceMode2D.Impulse);
                rbody.AddTorque(randomTorque, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Instantiating something without a rigidbody, check the passed GameObject");
                Destroy(spawnedItem);
            }
            Destroy(spawnedItem, f_itemLifetime);
        }
    }
}

