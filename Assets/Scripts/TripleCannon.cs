using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TripleCannon : MonoBehaviour {
    [SerializeField] private float powerUpDuration = 10f;
    private GameManager gameManager;
    private int scoreAddition;
    private float timer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isBlinking = false;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        scoreAddition = 2;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer >= 7 && !isBlinking) {
            StartCoroutine(BlinkEffect());
        }

        if (timer > 10) {
            Destroy(gameObject);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ShootingSystem shootingSystem = other.GetComponent<ShootingSystem>();

            if (shootingSystem != null) {
                shootingSystem.ActivateTripleShot(powerUpDuration);
            }
            gameManager.IncrementScore(scoreAddition);
            Destroy(gameObject);
        }
    }

    private IEnumerator BlinkEffect() {
        isBlinking = true;
        float blinkInterval = 0.2f; 
        float minOpacity = 0.3f;
        float maxOpacity = 1f;

        while (timer <= 10) {
            SetSpriteOpacity(minOpacity);
            yield return new WaitForSeconds(blinkInterval);

            SetSpriteOpacity(maxOpacity);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void SetSpriteOpacity(float opacity) {
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = opacity; // Alpha es el componente que controla la opacidad
            spriteRenderer.color = color;
        }
    }
}
