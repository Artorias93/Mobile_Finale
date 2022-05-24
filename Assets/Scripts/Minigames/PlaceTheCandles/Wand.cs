using UnityEngine;

namespace PlaceTheCandles
{
    public class Wand : MonoBehaviour
    {
        [Header("Wand Parameters")]
        [SerializeField] private float f_moveSpeed;
        [SerializeField] private float f_minBound;
        [SerializeField] private float f_maxBound;

        [Header("References")]
        [SerializeField] private GameObject obj_spell;

        private Animator anim;
        float f_dirX;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        { 
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Shot");
                Instantiate(obj_spell, transform.position + new Vector3(0, 0.2f, 0), Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, -180)));
            }

            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) f_dirX = Input.GetAxis("Horizontal") * ((f_moveSpeed-5f) * Time.deltaTime);
            else f_dirX = Input.acceleration.x * (f_moveSpeed * Time.deltaTime);

            transform.position += new Vector3(f_dirX, 0, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, f_minBound, f_maxBound), transform.position.y, transform.position.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("EnemyAttack"))
            {
                anim.SetTrigger("TakeDmg");
                
            }
        }
    }
}
