using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour {
    GameManager gameManager;
    private int healthAddition;
    private int scoreAddition;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        healthAddition = 25;
        scoreAddition = 1;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            gameManager.IncreaseHealth(healthAddition);
            gameManager.IncrementScore(scoreAddition);
            Destroy(gameObject);
        }
    }
}
