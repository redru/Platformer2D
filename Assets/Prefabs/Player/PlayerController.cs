using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int life = 100;
	public float speed = 5f;
	public float jumpForce = 100f;
    public int attackPower = 10;
    public AudioSource audioAttack;
    public Transform attackPoint;

	private Rigidbody2D rb;
	private Animator anim;
    private LayerMask attackMask = -1;
    private float rotation = 1f; // Needed to move correctly when PJ is on platforms

    // Level elements
    private DynamicPlatformController platform = null;

    // Player states
    private bool grounded = true;
    private bool dead = false;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        attackMask = LayerMask.GetMask("Enemy");
    }

	void Update () {
        // Remove all PJ logic when is dead
        if (!dead) {

            // If is press Space button, jump and set Animation
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                anim.SetBool("Jumping", true);
            }

            // Get left mouse click, handle attack and set Animation
            if (Input.GetMouseButtonDown(0))
            {
                if (rb.velocity.x != 0f || !grounded)
                {
                    audioAttack.Play();
                    anim.SetTrigger("RunAttack");
                    checkAttackCollision(Physics2D.OverlapCircleAll(attackPoint.position, 0.20f, attackMask.value));
                }
                else
                {
                    audioAttack.Play();
                    anim.SetTrigger("IdleAttack");
                    checkAttackCollision(Physics2D.OverlapCircleAll(attackPoint.position, 0.20f, attackMask.value));
                }

            }

            // Checks if PJ is dead
            checkDeath();
        }
    }

	void FixedUpdate() {
        // Remove all PJ logic when is dead
        if (!dead) {

            // A - D keypress check
            float h = Input.GetAxis("Horizontal");

            // Ground check
            grounded = (rb.velocity.y > -0.001f && rb.velocity.y < 0.001f);

            if (grounded) {
                anim.SetBool("Jumping", false);
            }

            if (h != 0f) {
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
            } else {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                anim.SetBool("Running", false);
            }
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
        if (coll.gameObject.tag == "DynamicPlatform")
        {
            platform = null;
        }
    }

    void OnGUI() {
        // Show reset button if dead
        if (dead) {
            if (GUI.Button(new Rect((Screen.width / 2) - 60, (Screen.height / 2) - 20, 120, 40), "RESET LEVEL")) {
                Application.LoadLevel("title_screen");
            }
        }
    }

    // If dead, remove gravity, stop showing the sprite and set his velocity to Zero
    void checkDeath() {
        if (transform.position.y < -2f || life <= 0) {
            dead = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
        }
    }

    // Deal damage when attack collides with other colliders
    void checkAttackCollision(Collider2D[] colliders) {
        foreach (Collider2D coll in colliders) {
            IDamageable dmged = coll.gameObject.GetComponent<IDamageable>();
            dmged.damage(attackPower);
        }
    }

}
