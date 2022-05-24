using UnityEngine;


namespace PlaceTheCandles
{
    public class Attack : MonoBehaviour
    {
        [Header("Attack Parameters")]
        [SerializeField] private float f_attackSpeed;
        
        [HideInInspector] public float f_speedMultiplier;
        [HideInInspector] public bool b_bossAttack;


        private Animator anim;
        public GameObject obj_player;
        private Rigidbody2D rb;
        private Vector3 startPos;
        private Vector3 dir;

        bool b_destroyed = false;
        int i_collId = -1;
        int i_charIdentifier = 0;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            startPos = transform.position;
            rb = GetComponent<Rigidbody2D>();

            if (b_bossAttack) obj_player = GameObject.FindGameObjectWithTag("Player");

            if (gameObject.CompareTag("PlayerAttack")) i_charIdentifier = 1;
            else
            {
                i_charIdentifier = -1;
                f_attackSpeed = f_attackSpeed * f_speedMultiplier;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (!b_destroyed)
            {
                if (b_bossAttack)
                {
                    dir = (obj_player.transform.position - startPos).normalized;
                    rb.velocity = -dir * (f_attackSpeed * Time.deltaTime * i_charIdentifier * 750);

                    float f_angleRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion q_rotation = new Quaternion();
                    q_rotation.eulerAngles = new Vector3(0, 0, f_angleRot + 90);
                    transform.rotation = q_rotation;
                }
                else transform.position += Vector3.up * (f_attackSpeed * Time.deltaTime * i_charIdentifier);
            }
            else if (i_charIdentifier == 1) transform.position = transform.parent.position;
            else rb.velocity = Vector2.zero;

            var pos = Camera.main.WorldToScreenPoint(transform.position);
            bool b_outOfBounds = !Screen.safeArea.Contains(pos);

            if (b_outOfBounds) Destroy(gameObject, 0.2f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) i_collId = 0;
            else if (collision.CompareTag("Enemy")) i_collId = 1;
            else if (collision.CompareTag("Boss")) i_collId = 2;
            else if (collision.CompareTag("PlayerAttack")) i_collId = 3;
            else if (collision.CompareTag("BossAttack")) i_collId = 4;

            if ((i_collId == 0 && i_charIdentifier == -1) || ((i_collId == 1 || i_collId == 2 || i_collId == 4) && i_charIdentifier == 1) || (i_collId == 3 && b_bossAttack))
            {
                GetComponent<Collider2D>().enabled = false;
                b_destroyed = true;
                
                if (i_charIdentifier == 1)
                {
                    transform.SetParent(collision.transform);
                    if (i_collId == 2) PlaceCandlesGameManager.Instance.CountError(-3);
                    if (i_collId == 4) anim.SetFloat("Speed", 0.25f);
                    else anim.SetFloat("Speed", 3);
                }

                if (i_charIdentifier == -1)
                {
                    if (b_bossAttack && i_collId == 0) PlaceCandlesGameManager.Instance.CountError(3);
                    else PlaceCandlesGameManager.Instance.CountError(1);
                }

                anim.SetTrigger("Hit");
            }
        }

        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}

