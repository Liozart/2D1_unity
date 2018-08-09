using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : MonoBehaviour {

	public Rigidbody2D rigidbod;
	Animator controllerAnim;
	public GameObject particle_Death;
	public SpriteRenderer sprite;
	bool isFacingRight = true;

	GameObject player;
	public LayerMask playerLayer;

	public int health = 3;
	public float speed = 2f;
	public int strengh = 1;
	public float fieldOfView = 3f;
	public float range = 0.3f;
	public string type = "Slime";

	bool isPlayerInSight = false;
	bool isPlayerInRange = false;
	bool isPlayerToTheRight = false;
	bool isHit = false;
	bool dead = false;
	bool isInvulnerable = false;
	float invulnerableTime = 0.5f;
	public bool isAttacking = false;

	public GameObject effectHit;

	// Use this for initialization
	void Start () {
		this.rigidbod = gameObject.GetComponent<Rigidbody2D>();
		this.controllerAnim = gameObject.GetComponent<Animator>();
		this.sprite = gameObject.GetComponent<SpriteRenderer>();
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		BasicAI();
		if (dead)
		if (!controllerAnim.GetCurrentAnimatorStateInfo(0).IsName("slimeHit")) {
			GameObject g = Instantiate(particle_Death, transform.position, transform.rotation);
			g.GetComponent<ParticleSystem>().Play();
			GameObject.Destroy(this.gameObject);
		}
	}

	void FixedUpdate() {
		if (!controllerAnim.GetCurrentAnimatorStateInfo(0).IsName("slimeHit")) {
			controllerAnim.SetFloat("Speed", Mathf.Abs(rigidbod.velocity.x));
			isHit = false;
		}
		if (!controllerAnim.GetCurrentAnimatorStateInfo(0).IsName("slimeAttack"))
			isAttacking = false;

	}

	/// <summary>
	/// Basics AI
	/// 
	//Check is player is in view
	//	If player is in range
	//		Attack
	//	Else
	//		Move to player
	//Else
	//	Idle
	/// </summary>
	void BasicAI(){
		isPlayerInSight = Physics2D.OverlapCircle(transform.position, fieldOfView, playerLayer);
		isPlayerInRange = Physics2D.OverlapCircle(transform.position, range, playerLayer);
		if (isPlayerInSight) {
			isPlayerToTheRight = ((player.transform.position.x - rigidbod.transform.position.x) > 0f) ? true : false;
			if (isPlayerToTheRight && !isFacingRight)
				Flip();
			if (!isPlayerToTheRight && isFacingRight)
				Flip();
			if (!isHit && !isInvulnerable) {
				if (isPlayerToTheRight)
					rigidbod.velocity = speed * Vector2.right;
				else
					rigidbod.velocity = speed * Vector2.left;
				if (isPlayerInRange && !isAttacking)
					Attack();
			}
		} 
	}

	public void Damage(int str) {
		if (!isHit && !isInvulnerable) {
			isHit = true;
			isInvulnerable = true;
			controllerAnim.SetTrigger("Hit");
			StartCoroutine(Invulnerable());
			health -= str;
			isAttacking = false;
			if (health <= 0)
				dead = true;
		}
	}

	void Attack() {
		controllerAnim.SetTrigger("Attack");
		isAttacking = true;
	}

	void Flip(){
		isFacingRight = !isFacingRight;
		sprite.flipX = !isFacingRight;
	}

	IEnumerator Invulnerable()
	{
		GameObject.Instantiate(effectHit, transform.position, transform.rotation);
		yield return new WaitForSeconds(invulnerableTime);
		isInvulnerable = false;
	}
}
