using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform targetObject;
    private Vector3 difference;
    private Vector3 velocity = Vector3.zero;

    void Start () {
        difference = transform.position - targetObject.position;
	}
	
	void Update () {
	
	}

    void FixedUpdate(){

    }

    void LateUpdate() {
        // transform.position = targetObject.position + difference;
        transform.position = Vector3.SmoothDamp(transform.position, targetObject.position + difference, ref velocity, 0.5f);
    }

}