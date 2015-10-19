using UnityEngine;
using System.Collections;

public class Boss1Controller : MonoBehaviour, IDamageable
{

    public int life = 100;
    public float speed = 5f;
    public float jumpForce = 50f;
    public int attackPower = 25;
    public Transform leftLimit;
    public Transform rightLimit;

    private Animator anim;
    private Rigidbody2D rigidBody;
    private Vector2 startingPosition;

    private GameObject player;

    private bool dead = false;
    private bool moving = false;
    private bool defending = false;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

	void Update () {
        if (anim.enabled && !anim.GetCurrentAnimatorStateInfo(0).IsName("Creating")) {
            checkDeath();
            
            //if (!moving) {
                Vector2 dir = (new Vector3(Random.Range(leftLimit.position.x, rightLimit.position.x), transform.position.y) - transform.position).normalized;
                print(dir);
                rigidBody.velocity = dir;
                moving = true;
            //}
        }
        
    }

    void FixedUpdate() {
        rigidBody.velocity = rigidBody.position.x > leftLimit.position.x && rigidBody.position.x < rightLimit.position.x ? rigidBody.velocity : new Vector2(0f, rigidBody.velocity.y);

        moving = rigidBody.velocity.x != 0f;
    }

    public int damage(int damageTaken) {
        life -= damageTaken;

        return damageTaken;
    }

    public void checkDeath() {
        if (life <= 0 && !dead) {
            dead = true;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

}
