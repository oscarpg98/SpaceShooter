using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void StartNewGame() {
        SceneManager.LoadScene("Level_1");
    }
    public void ExitGame() {
        Application.Quit();
    }
}
