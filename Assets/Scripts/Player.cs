using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Collider modifiers for overhead attack
public class ColliderParams {
	public ColliderParams(float ox, float oy, float sx, float sy) {
		offsetx = ox; offsety = oy;
		sizex = sx; sizey = sy;
	}
	public float offsetx;
	public float offsety;
	public float sizex;
	public float sizey;
}

public class Player : MonoBehaviour {

	public Rigidbody2D rigidbod;
	SpriteRenderer sprite;
	Animator animController;
	public GameObject cam;
	CameraFollowPlayer camscript;

	public bool isFacingRight = true;
	public bool isWalking = false;
	public bool isRunning = false;
	public bool isGrounded = false;
	public bool isAttacking = false;
	public Transform groundCheck;
	public LayerMask groundsLayers;

	public BoxCollider2D[] boxColliders;
	const int COLLIDER_BODY = 0;
	const int COLLIDER_ATTACK = 1;
	float COLLIDER_ATTACK_XOFFSET = 0.055f;
	bool isColliderAttackSet = false;
	ColliderParams colliderParamsNormal;
	ColliderParams colliderParamsFinalAttack;

	public Transform magicHandRight;
	public Transform magicHandLeft;
	public GameObject fireball;
	float fireballcooldowntime = 2;
	bool fireballcooldown = false;

	public int health = 100;
	public Inventory inventory;
	int strengh = 1;
	float pushStrengh = 100f;
	AttackType attackType;
	public GameObject deathParticles;
	public GUI_Death guideath;

	float maxWalkSpeed = 3f;
	float maxRunningAccel = 2.5f;
	float runningAccel = 0.5f;
	float jumpForce = 5000f;

	// Use this for initialization
	void Start () {
		this.rigidbod = GetComponent<Rigidbody2D>();
		this.boxColliders = GetComponents<BoxCollider2D>();
		this.sprite = GetComponent<SpriteRenderer> ();
		this.animController = GetComponent<Animator> ();
		this.inventory = GetComponent<Inventory>();
		this.camscript = cam.GetComponent<CameraFollowPlayer>();
		boxColliders[COLLIDER_BODY].name = "Boby";
		boxColliders[COLLIDER_ATTACK].name = "Attack1";
		colliderParamsNormal = new ColliderParams(boxColliders[COLLIDER_ATTACK].offset.x, boxColliders[COLLIDER_ATTACK].offset.y, 
			boxColliders[COLLIDER_ATTACK].size.x, boxColliders[COLLIDER_ATTACK].size.y);
		colliderParamsFinalAttack = new ColliderParams(0.2289857f, 0.4194852f, 0.7408531f, 0.8389704f);

		inventory.AddItem(new Item_Weapon("Shitty sword", 3, 1, 0));
		inventory.AddItem(new Item("Key", 0, 0));
	}
	
	// Update
	void FixedUpdate () {

		//Is it on ground
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundsLayers);
		animController.SetBool("Grounded", isGrounded);

		//Movement sprites
		float direction = Input.GetAxis ("Horizontal");
		isWalking = (direction == 0) ? false : true;
		float sprint = Input.GetAxis ("Fire3");
		isRunning = (sprint == 0) ? false : true;

		animController.SetFloat("Speed", Mathf.Abs(direction));

		if (isWalking) {
			if (isRunning) {
				if (runningAccel < maxRunningAccel)
					runningAccel += 0.01f;
			} else {
				runningAccel = 0.5f;
			}

			//Flip to the direction
			if (!isAttacking) {
				if (direction > 0 && !isFacingRight)
					FlipPlayer();
				else if (direction < 0 && isFacingRight)
					FlipPlayer();
			}
		}
			
		//Movement physic
		float speedfactor = maxWalkSpeed + (sprint * runningAccel);
		rigidbod.velocity = Vector2.right * (direction * speedfactor);

		//Jump
		float jump = Input.GetAxis("Jump");
		if ((jump > 0) && isGrounded) {
			animController.SetBool("Grounded", false);
			//rigidbod.velocity(new Vector2(0f, jumpForce), ForceMode2D.Force);
			rigidbod.velocity = Vector2.up * 100f;
		}
	}

	void Update() {
		float atk1 = Input.GetAxis ("Fire1");
		float atk2 = Input.GetAxis("Fire2");
		float magic = Input.GetAxis("Fire3");
		if (!animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack1") &&
			!animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack2") &&
			!animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack3") && 
			!animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack3_1")) {
			if (magic > 0 && !fireballcooldown) {
				animController.SetTrigger("Magic");
				float dir;
				GameObject f;
				if (isFacingRight) {
					dir = 1;
					f = GameObject.Instantiate(fireball, magicHandRight.position, magicHandRight.rotation);
				} else {
					dir = -1;
					f = GameObject.Instantiate(fireball, magicHandLeft.position, magicHandLeft.rotation);
				}
				f.GetComponent<Rigidbody2D>().velocity = new Vector2(dir * 5f, 0);
				StartCoroutine(FireBallCoolDown());
			}
			if (atk1 > 0 || atk2 > 0) {
				isAttacking = true;
				if (atk1 > 0) {
					animController.SetTrigger("Attack1");
					attackType = AttackType.Normal;
				}
				if (atk2 > 0) {
					animController.SetTrigger("Attack2");
					attackType = AttackType.Strong;
				}
			} else
				isAttacking = false;
		} else {
			if (atk1 > 0) {
				isAttacking = true;
				attackType = AttackType.Normal;
			}
			else
				isAttacking = false;
			animController.SetBool("StillAttacking", isAttacking);
		}
		if (animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack3") ||
			animController.GetCurrentAnimatorStateInfo(0).IsName("warriorAttack3_1")) {
			if (!isColliderAttackSet) {
				isColliderAttackSet = true;
				boxColliders[COLLIDER_ATTACK].offset = new Vector2(colliderParamsFinalAttack.offsetx, colliderParamsFinalAttack.offsety);
				boxColliders[COLLIDER_ATTACK].size = new Vector2(colliderParamsFinalAttack.sizex, colliderParamsFinalAttack.sizey);
			}
		} else if (isColliderAttackSet) {
			isColliderAttackSet = false;
			boxColliders[COLLIDER_ATTACK].offset = new Vector2(colliderParamsNormal.offsetx, colliderParamsNormal.offsety);
			boxColliders[COLLIDER_ATTACK].size = new Vector2(colliderParamsNormal.sizex, colliderParamsNormal.sizey);
		}
	}

	//Flipping the player
	void FlipPlayer(){
		isFacingRight = !isFacingRight;
		sprite.flipX = !isFacingRight;
		//Flip attack 1 & 2 collider
		COLLIDER_ATTACK_XOFFSET *= -1f;
		boxColliders[COLLIDER_ATTACK].offset = new Vector2(COLLIDER_ATTACK_XOFFSET, boxColliders[COLLIDER_ATTACK].offset.y);
	}


	//Make the player death
	void Dying() {
		sprite.enabled = false;
		boxColliders[COLLIDER_BODY].enabled = false;
		GameObject g = GameObject.Instantiate(deathParticles);
		g.GetComponent<ParticleSystem>().Play();
		camscript.isPlayerDead = true;
		guideath.isPlayerDead = true;
	}

	void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy") {
			Enemy_Slime enemy = col.gameObject.GetComponent<Enemy_Slime>();
			if (enemy.isAttacking) {
				camscript.Shake();
				health -= enemy.strengh;
				int dir = (isFacingRight) ? 1 : -1;
				rigidbod.AddForce(new Vector2(dir * enemy.strengh * 100, 0));
				if (health == 0) {
					Dying();
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			Enemy_Slime enemy = col.gameObject.GetComponent<Enemy_Slime>();
			if (isAttacking) {
				int mult = (attackType == AttackType.Strong) ? 2 : 1;
				enemy.Damage(strengh * mult);
				int dir = (isFacingRight) ? 1 : -1;
				col.attachedRigidbody.AddForce(new Vector2(dir * pushStrengh, 0));
				isAttacking = false;
			}
		}
	}

	IEnumerator FireBallCoolDown() {
		fireballcooldown = true;
		yield return new WaitForSeconds(fireballcooldowntime);
		fireballcooldown = false;
	}
}
