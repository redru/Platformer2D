using UnityEngine;
using System.Collections;

public class GameStateVars : MonoBehaviour {

    public static int playerCurrentLifes = 3;
    public static string currentLevel = "title_screen";

    void Awake() {
        DontDestroyOnLoad(this);
    }

}
