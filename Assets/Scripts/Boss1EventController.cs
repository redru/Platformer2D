using UnityEngine;

public class Boss1EventController : MonoBehaviour {

    private GameObject boss;

	// Use this for initialization
	void Start () {
        boss = GameObject.FindGameObjectWithTag("Boss");
	}

    void OnTriggerEnter2D(Collider2D coll) {
        boss.GetComponent<SpriteRenderer>().enabled = true;
        boss.GetComponent<Animator>().enabled = true;
        Destroy(gameObject);
    }

}
