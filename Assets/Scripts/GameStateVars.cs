using UnityEngine;
using System.Collections;

public class GameStateVars : MonoBehaviour {

    public static int playerCurrentLifes = 3;
    public static string currentLevel = "title_screen";
    public static bool levelComplete = false;

    void Awake() {
        DontDestroyOnLoad(this);
    }

}
