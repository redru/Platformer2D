using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, IDamageable {

    public int life = 100;
    public int playerDefaultLifes = 3;
    public float speed = 5f;
	public float jumpForce = 100f;
    public int attackPower = 10;
    public float attackRate = 1f;
    public AudioSource audioAttack;
    public Transform attackPoint;

	private Rigidbody2D rb;
	private Animator anim;
    private LayerMask attackMask = -1;
    private float rotation = 1f; // Needed to move correctly when PJ is on platforms
    private float lastAttackMoment = 0f;

    // Level elements
    private DynamicPlatformController platform = null;

    // Player states
    private bool grounded = true;
    private bool dead = false;
    private bool stunned = false;
    private float stunTime = 0f;
    private float stunCount = 0f;
    private bool levelFinished = false;

	void Start () {
        rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        attackMask = LayerMask.GetMask("Enemy", "EnemyPlayerCollider");
    }

	void Update () {
        // Remove all PJ logic when is dead
        if (!dead && !levelFinished) {
            updateStun();

            // If is press Space button, jump and set Animation
            if (Input.GetKeyDown(KeyCode.Space) && grounded && !stunned) {
                jump(1f);
            }

            // Get left mouse click, handle attack and set Animation
            if (Input.GetMouseButtonDown(0) && !stunned && lastAttackMoment + attackRate <= (Time.time)) {
                attack();                
            }

            // Checks if PJ is dead
            checkDeath();
        }
    }

	void FixedUpdate() {
        // Remove all PJ logic when is dead
        if (!dead && !levelFinished) {

            // A - D keypress check
            float h = Input.GetAxis("Horizontal");

            // Ground check
            grounded = (rb.velocity.y > -0.001f && rb.velocity.y < 0.001f);

            if (grounded && !stunned) {
                anim.SetBool("Jumping", false);
            }

            if (h != 0f && !stunned) {
                rb.velocity = new Vector2(speed * h, rb.velocity.y);
                anim.SetBool("Running", true);

                // Character rotation based on the positiveness of the velocity
                if (h < 0f) {
                    transform.rotation = Quaternion.AngleAxis(180, Vector2.up);
                    rotation = -1f;
                } else if (h > 0f) {
                    transform.rotation = Quaternion.AngleAxis(0, Vector2.up);
                    rotation = 1f;
                }
            } else if (h == 0f && !stunned) {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                anim.SetBool("Running", false);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll) {
        // If collides with EnergyBall, stun and retreat
        if (coll.gameObject.tag == "EnergyBall") {
            Vector2 currentVel = rb.velocity;
            stun(0.25f);
            rb.velocity = currentVel * -2;
        } else if (coll.gameObject.name == "Boss1Event") {
            stun(1.5f);
        } else if (coll.gameObject.name == "LevelFinish") {
            levelFinished = true;
            stun(0f);
            GameStateVars.levelComplete = true;
        }

    }

    void OnCollisionStay2D(Collision2D coll) {
        // Case when PJ is on a platform
        if (coll.gameObject.tag == "DynamicPlatform" && platform == null) {
            platform = coll.gameObject.GetComponent<DynamicPlatformController> ();
        }

        if (platform != null) {
            transform.Translate(Vector3.right * rotation * platform.horizontalSpeed * platform.horizontalDirection * Time.deltaTime);
            transform.Translate(Vector3.up * rotation * platform.verticalSpeed * platform.verticalDirection * Time.deltaTime);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        // Case when PJ is on a platform
        if (coll.gameObject.tag == "DynamicPlatform") {
            platform = null;
        }
    }

    // Stuns the player for the given time
    void stun(float time) {
        stunTime = time;
        stunned = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("Running", false);
        anim.SetBool("Jumping", false);
    }

    void updateStun() {
        if (stunned && stunCount + Time.deltaTime < stunTime) {
            stunCount += Time.deltaTime;
        } else {
            stunCount = 0f;
            stunned = false;
        }
    }

    // If dead, remove gravity, stop showing the sprite and set his velocity to Zero
    void checkDeath() {
        if (transform.position.y < -2f || life <= 0) {
            // Update global application state
            GameStateVars.playerCurrentLifes -= 1;

            // Update player current state
            dead = true;           
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
        }
    }

    // Makes the player attack
    void attack() {
        checkAttackCollision(Physics2D.OverlapCircleAll(attackPoint.position, 0.16f, attackMask.value));
        audioAttack.Play();
        lastAttackMoment = Time.time;

        if (rb.velocity.x != 0f || !grounded) {
            anim.SetTrigger("RunAttack");
        } else {
            anim.SetTrigger("IdleAttack");
        }
    }

    public int damage(int damageTaken) {
        life -= damageTaken;
        return damageTaken;
    }

    void jump(float multiplier) {
        rb.AddForce(new Vector2(0f, jumpForce * multiplier), ForceMode2D.Impulse);
        anim.SetBool("Jumping", true);
    }

    // Deal damage when attack collides with other colliders
    void checkAttackCollision(Collider2D[] colliders) {
        foreach (Collider2D coll in colliders) {
            IDamageable dmged = coll.gameObject.GetComponent<IDamageable>();
            dmged.damage(attackPower);

            if (coll.name == "Boss_1") {
                stun(0.5f);
                rb.velocity = new Vector2(-speed * 2, speed * 2);
            }
        }
    }

    public bool isDead() {
        return dead;
    }

}
