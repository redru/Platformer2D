using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;

    private bool platformChecking = false;
    private Collider2D[] platformColliders;
    private GameObject[] platforms;
    private GameObject[] dynPlatforms;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        platforms = GameObject.FindGameObjectsWithTag("Platform");
        dynPlatforms = GameObject.FindGameObjectsWithTag("DynamicPlatform");

        // Set global state variables
        GameStateVars.currentLevel = "level0";

        if (GameStateVars.playerCurrentLifes <= 0) {
            GameStateVars.playerCurrentLifes = playerController.playerDefaultLifes;
        }

        platformColliders = new Collider2D[platforms.Length + dynPlatforms.Length];

        int counter = 0;
        foreach (GameObject tmp in platforms) {
            platformColliders[counter] = tmp.GetComponent<Collider2D>();
            counter++;
        }

        foreach (GameObject tmp in dynPlatforms) {
            platformColliders[counter] = tmp.GetComponent<Collider2D>();
            counter++;
        }
    }
	
	void Update () {
        if (!platformChecking) {
            StartCoroutine(changePlatformsTriggerState());
        }
    }

    // Change the isTrigger property of all platforms in the level, checking the position of the player
    IEnumerator changePlatformsTriggerState() {
        platformChecking = true;

        int counter = 0;
        foreach (GameObject tmp in platforms) {
            platformColliders[counter].isTrigger = tmp.transform.position.y >= (player.transform.position.y - 0.1f);
            counter++;
        }

        foreach (GameObject tmp in dynPlatforms) {
            platformColliders[counter].isTrigger = tmp.transform.position.y >= (player.transform.position.y - 0.1f);
            counter++;
        }

        yield return new WaitForSeconds(.1f);
        platformChecking = false;
    }

    void OnGUI() {
        // Help text with controls displayed top left
        GUI.Label(new Rect(10, 10, 200, 20), "INSTRUCTIONS");
        GUI.Label(new Rect(10, 30, 200, 20), "<-  LEFT = 'A'");
        GUI.Label(new Rect(10, 50, 200, 20), "-> RIGHT = 'D'");
        GUI.Label(new Rect(10, 70, 200, 20), "^   JUMP = 'SPACEBAR'");
        GUI.Label(new Rect(10, 90, 300, 20), "  ATTACK = 'LEFT MOUSE CLICK'");

        // The button when the player is dead
        if (playerController.isDead()) {
            if (GameStateVars.playerCurrentLifes > 0) {
                if (GUI.Button(new Rect((Screen.width / 2) - 60, (Screen.height / 2) - 20, 120, 40), "RETRY"))
                {
                    Application.LoadLevel("level0");
                }
            } else {
                if (GUI.Button(new Rect((Screen.width / 2) - 60, (Screen.height / 2) - 20, 120, 40), "GAME OVER"))
                {
                    Application.LoadLevel("title_screen");
                }
            }           
        }

        // Player state
        GUI.Label(new Rect(Screen.width - 110, 10, 100, 20), "LIFES: " + GameStateVars.playerCurrentLifes);

        if (GameStateVars.levelComplete) {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 1.5f, 500, 40), "GRAZIE MILLE PER AVER GIOCATO!");
        }

    }

}
