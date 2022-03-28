using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

	public GameObject obj_bladeTrailPrefab;
	public float f_minCuttingVelocity;
	bool b_isCutting = false;
	public int i_scoring;

	Vector2 v2_previousPosition;

	GameObject obj_currentBladeTrail;
	Rigidbody2D rb;
	Camera cam;
	CircleCollider2D circleCollider;

	void Start ()
	{
		cam = Camera.main;
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			StartCutting();
		} else if (Input.GetMouseButtonUp(0))
		{
			StopCutting();
		}

		if (b_isCutting)
		{
			UpdateCut();
		}

	}

	void FixedUpdate()
	{

		if (b_isCutting)
		{
			UpdateCut();
		}

	}

	void UpdateCut ()
	{
		Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
		transform.position = newPosition;
        rb.position = newPosition;

		float velocity = (newPosition - v2_previousPosition).magnitude * Time.deltaTime;
		if (velocity > f_minCuttingVelocity)
		{
			circleCollider.enabled = true;
		} else
		{
			circleCollider.enabled = false;
		}
		
		v2_previousPosition = newPosition;
	}

	void StartCutting ()
	{
		b_isCutting = true;
		obj_currentBladeTrail = Instantiate(obj_bladeTrailPrefab, transform);
		circleCollider.enabled = false;
	}

	void StopCutting ()
	{
		b_isCutting = false;
		obj_currentBladeTrail.transform.SetParent(null);
		Destroy(obj_currentBladeTrail);
		circleCollider.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.CompareTag("Item"))
		{
			i_scoring += 100;
		}
	}

}
