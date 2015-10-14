using UnityEngine;
using System.Collections;

public class TitleScreenManager : MonoBehaviour {

	void OnGUI() {
        if (GUI.Button(new Rect((Screen.width / 2) - 60, (Screen.height / 2) + 50, 120, 40), "PLAY")) {
            Application.LoadLevel("level0");
        }
    }
}
