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
		float moveSpeed = 5f, jumpForce = 600f, fallGravityMultiplier;
        public float gravityScale;
		[SerializeField] Animator animator;
		public float Speed
		{
			get { return moveSpeed; }
			set { moveSpeed = (int)value; }
		}
		private void Start()
        {
			
			gravityScale=rb.gravityScale;
        }
        void FixedUpdate()
		{
			rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

			if (rb.velocity.y < 0)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else
			{
				rb.gravityScale = gravityScale;
			}
		}
        private void Update()
        {
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


	}
}
