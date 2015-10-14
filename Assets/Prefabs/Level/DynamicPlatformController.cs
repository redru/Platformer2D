using UnityEngine;

public class DynamicPlatformController : MonoBehaviour {

    public float horizontalRange = 1f;
    public float horizontalSpeed = 1f;
    public float horizontalDirection = 1f;

    public float verticalRange = 1f;
    public float verticalSpeed = 1f;
    public float verticalDirection = 1f;

    private Vector3 startingPosition = Vector3.zero;

    void Start () {
        startingPosition = transform.position;
    }
	
	void Update () {
        transform.Translate (Vector3.right * horizontalSpeed * horizontalDirection * Time.deltaTime);

        if (transform.position.x > startingPosition.x + horizontalRange || transform.position.x < startingPosition.x - horizontalRange) {
            horizontalDirection = -horizontalDirection;
        }

        transform.Translate(Vector3.up * verticalSpeed * verticalDirection * Time.deltaTime);

        if (transform.position.y > startingPosition.y + verticalRange || transform.position.y < startingPosition.y - verticalRange) {
            verticalDirection = -verticalDirection;
        }
    }

}
