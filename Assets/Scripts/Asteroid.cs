using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class Asteroid : MonoBehaviour {
    private GameManager gameManager;
    private float velocidad = 15;
    private float timer;
    private bool isReleased = false;
    private ShootingSystem shootingSystem;
    private ObjectPool<Asteroid> asteroidPool;
    public ObjectPool<Asteroid> AsteroidPool { get => asteroidPool; set { asteroidPool = value; } }
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private int scoreAddition;

    void Start () {
        gameManager = FindObjectOfType<GameManager>();
        shootingSystem = FindObjectOfType<ShootingSystem>();
        scoreAddition = 1;
    }

    private void Update() {
        transform.Translate(Time.deltaTime * velocidad * -transform.right);

        timer += Time.deltaTime;
        if (timer >= 4) {
            asteroidPool.Release(this);
            timer = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Laser")) {
            gameManager.IncrementScore(scoreAddition);
            // Añado esta comprobación porque cuando disparo 3 lasers colisiona más de 1 con el asteroide e intenta hacer release cuando ya está desactivado.
            if (!isReleased) {
                isReleased = true;
                asteroidPool.Release(this);
            }
            shootingSystem.ReleaseLaserExternally(other.GetComponent<Laser>());
            AudioManager.Instance.PlaySoundEffect(crashSound);
            float powerUpProbability = Random.value;
            if (powerUpProbability <= 0.25f) {
                Instantiate(powerUps[0], transform.position, Quaternion.identity); // Medkit
            }
            else if (powerUpProbability <= 0.40f) {
                Instantiate(powerUps[1], transform.position, Quaternion.identity); // Triple Cannon
            }
        }
        else if (other.gameObject.CompareTag("Player")) {
            asteroidPool.Release(this);
            AudioManager.Instance.PlaySoundEffect(crashSound);
        }
    }
}