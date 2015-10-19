using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject targetObject;
    private Rigidbody2D targetRb;

    private Vector3 difference;
    private Vector3 velocity = Vector2.zero;

    void Start () {
        targetRb = targetObject.GetComponent<Rigidbody2D>();
        difference = transform.position - targetObject.transform.position;
	}

    void LateUpdate() {
        move(targetRb.velocity / 2f);
    }

    void move(Vector3 vel) {
        transform.position = Vector3.SmoothDamp(transform.position, targetObject.transform.position + difference, ref velocity, 0.25f);
    }

}