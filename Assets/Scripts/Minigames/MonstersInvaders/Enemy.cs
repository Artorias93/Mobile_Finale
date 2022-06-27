using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MonstersInvaders
{
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Parameters")]
        [SerializeField] private float f_enemySpeed;
        [SerializeField] private float f_fireRate;

        [Header("References")]
        [SerializeField] private GameObject obj_attack;

        [HideInInspector] public float f_speedMultiplier;
        [HideInInspector] public float f_fireRateMultiplier;
        [HideInInspector] public float f_attackSpeedMultiplier;

        private Animator anim;
        bool b_wentDown = false;
        bool b_isBoss = false;
        float f_projRotation;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            if (CompareTag("Boss"))
            {
                b_isBoss = true;
                f_speedMultiplier *= 1.5f;
                f_fireRateMultiplier *= 5f;
            }
            StartCoroutine(ShotProjectile(2f));
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (MonsInvGameManager.Instance.GameFinished)
            {
                Destroy(anim);
                return;
            }

             transform.position += Vector3.right * (f_enemySpeed * Time.deltaTime * f_speedMultiplier);

            if (transform.position.x < -6 || transform.position.x > 6)
            {
                if (!b_wentDown)
                {
                    b_wentDown = true;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    if (!b_isBoss) transform.position = new Vector3(Mathf.Sign(transform.position.x) * 6, transform.position.y - GetComponentInParent<GridLayoutGroup>().cellSize.y/2, transform.position.z);
                }

                f_enemySpeed = -f_enemySpeed;
            }
            else b_wentDown = false;
        }

        private IEnumerator ShotProjectile(float f_delayTime)
        {
            while(true)
            {
                yield return new WaitForSeconds(Random.Range(1,5));
                if (MonsInvGameManager.Instance.GameFinished) yield break;
                if (Random.value < f_fireRate * f_fireRateMultiplier) anim.SetTrigger("Attack");
            }
        }

        public void SpawnProjectile()
        {
            if (b_isBoss) f_projRotation = 0;
            else f_projRotation = -90;
            GameObject obj_projectile = Instantiate(obj_attack, transform.position + new Vector3(0, 0, 0), Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, f_projRotation)));
            obj_projectile.GetComponent<Attack>().f_speedMultiplier = f_attackSpeedMultiplier;
            obj_projectile.GetComponent<Attack>().b_bossAttack = b_isBoss;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerAttack"))
            {
                if (b_isBoss) f_enemySpeed = 0;
                GetComponent<Collider2D>().enabled = false;
                anim.SetTrigger("TakeDmg");
            }
            if (collision.CompareTag("Finish")) MonsInvGameManager.Instance.LoseBehaviour(true);
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}

