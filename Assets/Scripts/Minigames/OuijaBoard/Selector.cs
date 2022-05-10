using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace OuijaBoardMinigame
{
	public class Selector : MonoBehaviour
	{	
		[Header("Selector Parameters")]
		[SerializeField] private float f_moveSpeed;
		[SerializeField] private float f_tollerance = 0.1f;
		[SerializeField] private float f_startingTimeToStay = 3f;
		[SerializeField] private bool b_resetTime;

		[Header("References")]
		[SerializeField] private BoardManager m_ouijaManager;


		Rigidbody2D rb;
		private float f_baseMoveSpeed;
		float f_dirX;
		float f_dirY;
		public List<string> letters = new List<string>();
		float f_timeToStay;
		private bool b_isFrozen = false;

		// Use this for initialization
		void Start()
		{
			f_timeToStay = f_startingTimeToStay;
			rb = GetComponent<Rigidbody2D>();
			f_baseMoveSpeed = f_moveSpeed;
		}

		// Update is called once per frame
		void Update()
		{
			f_dirX = Input.acceleration.x * f_moveSpeed;
			f_dirY = Input.acceleration.y * f_moveSpeed;
		}

		void FixedUpdate()
		{
			if (!b_isFrozen)
			{
				rb.velocity = new Vector2(f_dirX, f_dirY);
				transform.eulerAngles = new Vector3(0, 0, -f_dirX * 7);
			}
			else
			{
				rb.velocity = Vector2.zero;
			}
		}

        private void OnTriggerStay2D(Collider2D collision)
        {
			float f_relativeVelocity = Vector3.Magnitude(collision.attachedRigidbody.velocity - rb.velocity);
			if (f_relativeVelocity <= f_tollerance) f_timeToStay -= Time.deltaTime;
			else
            {
				if (b_resetTime) f_timeToStay = f_startingTimeToStay;
				else f_timeToStay += Time.deltaTime;						// ---- > Condizione negativa, si può scegliere se resettare il timer, di assegnare un incremento della durata, o se non fare nulla
			}

			if (f_timeToStay <= 0)
			{
				f_timeToStay = f_startingTimeToStay;
				if (letters.Count == 0) letters.Add(collision.GetComponent<TMP_Text>().text);
				else letters.Add(collision.GetComponent<TMP_Text>().text.ToLower());
				collision.enabled = false;
				collision.GetComponent<TextMeshPro>().color = new Color32(15, 3, 0, 255);
				m_ouijaManager.CheckWord(letters.Count, string.Join("", letters.ToArray()));
			}
		}

		public void freezeSelector(bool b_isFreeze) {
			b_isFrozen = b_isFreeze;
		}
	}
}