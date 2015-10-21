using UnityEngine;
using System.Collections;

public class LevelFinishController : MonoBehaviour {

    private GameObject mainCamera;
    private AudioSource victory;

	void Start () {
        mainCamera = GameObject.Find("Main Camera");
        victory = GetComponent<AudioSource>();
    }
	
	void OnTriggerEnter2D(Collider2D coll) {
        mainCamera.GetComponent<AudioSource>().Stop();
        victory.Play();
    }

}
