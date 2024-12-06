using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public TextMeshProUGUI scoreText; 
    private int score;
    [SerializeField] private int health;
    [SerializeField] private int shield;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Image redScreen;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    private float flashDuration = 0.5f;
    private Color transparentColor = new Color(0f, 0f, 0f, 0f);
    private Color redColor = new Color(1f, 0f, 0f, 0.5f);
    private float timerLastHit;
    private float time2RegenerateShield;
    private Image healthBarImage;
    private Image shieldBarImage;
    [SerializeField] private float shieldRegenerationDuration;
    [SerializeField] private TextMeshProUGUI gameOverText;
    private bool isPaused = false;

    private void Start() {
        health = 100;
        shield = 100;
        score = 0;
        timerLastHit = 0f;
        time2RegenerateShield = 5f;
        healthBarImage = healthBar.GetComponent<Image>();
        shieldBarImage = shieldBar.GetComponent<Image>();
        shieldRegenerationDuration = 1f;

        if (redScreen != null) {
            redScreen.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (health <= 0) {
            GameOver();
        }

        timerLastHit += Time.deltaTime;
        if (shield < 100 && timerLastHit >= time2RegenerateShield) {
            StartCoroutine(RegenerateShield());
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void IncrementScore() {
        ++score;
        scoreText.text = "Score: " + score.ToString();
    }

    public void DecreaseHealth() {
        timerLastHit = 0f;
        if (shield > 0) {
            shield -= 100;
            shieldBarImage.fillAmount = (float)shield / 100f;
        }
        else {
            health -= 25;
            healthBarImage.fillAmount = (float)health / 100f;
        }
        FlashRedScreen();
    }

    private IEnumerator RegenerateShield() {
        timerLastHit = 0f;

        float startValue = shieldBarImage.fillAmount;
        float endValue = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < shieldRegenerationDuration) {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / shieldRegenerationDuration;
            shieldBarImage.fillAmount = Mathf.Lerp(startValue, endValue, time);
            yield return null;
        }

        shieldBarImage.fillAmount = endValue;
        shield = 100;
    }

    public void FlashRedScreen() {
        if (redScreen == null) return;
        StartCoroutine(FlashRedEffect()); // Me permite ejecutar la función durante varios frames
    }

    private IEnumerator FlashRedEffect() {
        redScreen.gameObject.SetActive(true);

        float timer = 0f;

        // Cambia la imagen roja de opacidad de transparente a semitransparente.
        while (timer < flashDuration) {
            // Utilizo unscaledDeltaTime para que la corutina pueda continuar cuando se para
            // el juego al hacer Game Over y no se quede el parpadeo a medias.
            timer += Time.unscaledDeltaTime;
            float effectDuration = timer / flashDuration;
            redScreen.color = Color.Lerp(redColor, transparentColor, effectDuration);
            yield return null; // Pausa la corutina para que se ejecute por partes durante unos cuantos frames hasta que acaba el efecto.
        }

        redScreen.color = transparentColor;
        redScreen.gameObject.SetActive(false);
    }

    private void TogglePause() {
        isPaused = !isPaused;

        if (isPaused) {
            gameOverText.text = "pause";
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            gameOverScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void GameOver() {
        if (gameOverScreen != null) {
            gameOverText.text = "game over";
            gameOverScreen.SetActive(true);
        }

        Time.timeScale = 0; // Pausa el juego
    }

    public void RestartGame() {
        Time.timeScale = 1; // Reanuda el juego
        health = 100;
        score = 0;
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene("Level_1");
    }
    public void MainMenu() {
        RestartGame();
        SceneManager.LoadScene("MainMenu");
    }
}