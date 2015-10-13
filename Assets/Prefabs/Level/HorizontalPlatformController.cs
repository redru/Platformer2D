using UnityEngine;
using System.Collections;

public class HorizontalPlatformController : MonoBehaviour {

    public float dynamicRange = 1f;
    public float speed = 1f;
    public float direction = 1f;

    private Vector3 startingPosition = Vector3.zero;

    void Start () {
        startingPosition = transform.position;
    }
	
	void Update () {
        transform.Translate (Vector3.right * speed * direction * Time.deltaTime);

        if (transform.position.x > startingPosition.x + dynamicRange || transform.position.x < startingPosition.x - dynamicRange) {
            direction = direction * -1f;
        }
	}

}
