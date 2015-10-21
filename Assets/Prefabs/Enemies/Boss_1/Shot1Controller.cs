using UnityEngine;
using System.Collections;

public class Shot1Controller : MonoBehaviour {

    public float speed = 1f;
    public int direction = 1;
    public int damage = 10;
    public int rotation = 0;
    public float lifeTime = 5f;
    private float lifeCount = 0f;

	// Use this for initialization
	public void Initialize(float speed, int direction, float lifeTime, int damage) {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;

        if (direction == 1) {
            transform.rotation = Quaternion.AngleAxis(180, Vector2.up);
        } else {
            transform.rotation = Quaternion.AngleAxis(0, Vector2.up);
        }
    }

    void Update() {
        lifeCount += Time.deltaTime;

        if (lifeCount >= lifeTime)
            Destroy(gameObject);
    }

    void FixedUpdate() {
        transform.Translate(new Vector2(speed * Time.deltaTime * direction, 0f));
    }
	
	void OnTriggerEnter2D(Collider2D coll) {
        coll.gameObject.GetComponent<IDamageable>().damage(damage);
        Destroy(gameObject);
    }
}
