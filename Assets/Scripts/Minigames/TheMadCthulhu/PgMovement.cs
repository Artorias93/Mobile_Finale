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
		float moveSpeed = 5f, jumpForce = 600f;


		void Start()
		{


		}

		void Update()
		{
			
		}

		void FixedUpdate()
		{
			rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
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
				rb.AddForce(Vector2.up * jumpForce);
		}

		public void StopMoving()
		{
			dirX = 0;
			rb.velocity = Vector2.zero;
		}


	}
}
