using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheMadCthulhuMinigame
{
	public class PgMovement : MonoBehaviour
	{

		[SerializeField]
		private Rigidbody2D rb;
		float dirX;
		[SerializeField]
		private GameObject pg;
		[SerializeField]
		float moveSpeed = 5f, jumpForce = 600f, fallGravityMultiplier, repellingForce = 10f;
        [SerializeField] private float gravityScale;
		[SerializeField, Tooltip("Time in which the player can't move after taking damage")] 
		private float DamageCooldownTime = 0.5f;
		private float ElapsedTime = 0;

		[SerializeField] Animator animator;
		public float Speed
		{
			get { return moveSpeed; }
			set { moveSpeed = (int)value; }
		}
		private void Start()
        {
			gravityScale=rb.gravityScale;
			rb.gravityScale = gravityScale * fallGravityMultiplier;

			ElapsedTime = DamageCooldownTime;
		}
        void FixedUpdate()
		{
			if(ElapsedTime >= DamageCooldownTime)
				rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
		}
        private void Update()
        {
			if(ElapsedTime <= DamageCooldownTime)
            {
				ElapsedTime += Time.deltaTime;
				dirX = 0;
            }

			Animate();
        }
        void Animate()
        {
			if (dirX == 0)
			{
				animator.SetBool("ToR", false);
				animator.SetBool("ToL", false);
				animator.SetBool("Jumping", false);


			}
			if (dirX > 0) 
			{ 
				animator.SetBool("ToR", true);
				animator.SetBool("ToL", false);
				animator.SetBool("Jumping", false);


			}
			if (dirX < 0) 
			{ 
				animator.SetBool("ToL", true);
				animator.SetBool("ToR", false);
				animator.SetBool("Jumping", false);

			}
			if (rb.velocity.y > 0)
			{
				animator.SetBool("ToL", false);
				animator.SetBool("ToR", false);
				animator.SetBool("Jumping", true);

			}
			

		}
		public void MoveRight()
		{

			dirX = 1;
			
		}
		public void MoveLeft()
		{

			dirX = -1;
			
		}
		public void Jump()
		{
			
			if (rb.velocity.y == 0) 
			{ 
				rb.AddForce(Vector2.up *jumpForce, ForceMode2D.Impulse);
				
			}
			
		}

		public void StopMoving()
		{

				dirX = 0;
				rb.velocity = Vector2.zero;
			
		}

        private void OnCollisionEnter2D(Collision2D collision)
        {
			if (collision.gameObject.CompareTag("Enemy"))
            {
				ElapsedTime = 0;	//Start damage cooldown
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
			if (collision.gameObject.CompareTag("Enemy"))
            {
                Vector3 EnemyPos = collision.transform.position;
                Vector3 PlayerPos = this.transform.position;
                Debug.DrawLine(EnemyPos, PlayerPos, Color.red, 3.0f);

				Vector3 forceDirection = PlayerPos - EnemyPos;
				forceDirection.Normalize();

                rb.AddForce(forceDirection * repellingForce, ForceMode2D.Force);
				
            }
        }
    }
}
