﻿using UnityEngine;
using System.Collections;

public class TitleScreenManager : MonoBehaviour {

    private GameStateManager gameStateManager;

    void Start() {
        GameStateVars.currentLevel = "title_screen";
    }

	void OnGUI() {
        if (GUI.Button(new Rect((Screen.width / 2) - 60, (Screen.height / 2) + 80, 120, 40), "PLAY")) {
            Application.LoadLevel("level0");
        }
    }
}
