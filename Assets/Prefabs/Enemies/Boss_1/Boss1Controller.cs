using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss1Controller : MonoBehaviour, IDamageable {

    public int life = 100;
    public float speed = 1f;
    public float jumpForce = 50f;
    public int attackPower = 25;
    public Transform leftLimit;
    public Transform rightLimit;
    public GameObject rangeAttackPosition;

    public Animator anim { get; set; }
    private Rigidbody2D rigidBody;
    private Vector2 startingPosition;
    private Vector3 targetPosition;

    private GameObject player;

    // Boss states
    private bool dead = false;
    private bool moving = false;
    private bool defending = false;
    
    private Attack mainAttack;
    private float nextMainAttack = 1f;
    private float mainAttackCounter = 0f;

    // Boss shots
    public List<GameObject> shots;

    // Animator states
    private bool creatingState = true;

    void Start() {
        // Attack startup
        mainAttack = new MainAttack(this);

        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

	void Update () {
        if (anim.enabled && !creatingState && !dead) {

            if (updateAttackLogic()) {
                rotate(false);
                mainAttack.executeAttack(1.6f, 0.5f);
                nextMainAttack = Random.Range(1.8f, 3.5f);
                mainAttackCounter = 0f;
            }

            if (!mainAttack.isExecuting()) {
                move();
                rotate();
            }

            checkDeath();
        }
        
    }

    public int damage(int damageTaken) {
        life -= damageTaken;
        targetPosition = transform.position;
        rotate(false);

        return damageTaken;
    }

    private void checkDeath() {
        if (life <= 0 && !dead) {
            dead = true;
            StartCoroutine(Die());
        }
    }

    private bool updateAttackLogic() {
        if (mainAttack.isReadyAttack()) {
            GameObject shot = (GameObject) Instantiate(shots[0], rangeAttackPosition.transform.position, Quaternion.identity);
            shot.GetComponent<Shot1Controller>().Initialize(2f, -1, 5f, attackPower);
            mainAttack.consumeReadyAttack();
        } else {
            mainAttackCounter += Time.deltaTime;
            return mainAttackCounter >= nextMainAttack;
        }

        return false;
    }

    private IEnumerator Die() {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(GameObject.Find("Boss1WallBlock"));
        Destroy(gameObject);
    }

    private void move() {
        if (!moving) {
            targetPosition = new Vector2(Random.Range(leftLimit.position.x, rightLimit.position.x), transform.position.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        moving = transform.position != targetPosition;
        anim.SetBool("Running", moving);
    }

    private void rotate() {
        if (targetPosition.x - transform.position.x < 0f) {
            transform.rotation = Quaternion.AngleAxis(180, Vector2.up);
        } else if (targetPosition.x - transform.position.x > 0f) {
            transform.rotation = Quaternion.AngleAxis(0, Vector2.up);
        }
    }

    private void rotate(bool toRight) {
        transform.rotation = toRight ? Quaternion.AngleAxis(0, Vector2.up) : Quaternion.AngleAxis(180, Vector2.up);
    }

    public void setCreating(bool creating) {
        this.creatingState = creating;
    }

}
