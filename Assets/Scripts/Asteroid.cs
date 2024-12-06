using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class Asteroid : MonoBehaviour {
    private GameManager gameManager;
    private float velocidad = 15;
    private float timer;
    private ObjectPool<Asteroid> asteroidPool;
    public ObjectPool<Asteroid> AsteroidPool { get => asteroidPool; set { asteroidPool = value; } }
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip crashSound;

    void Start () {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        transform.Translate(-transform.right * velocidad * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= 4) {
            asteroidPool.Release(this);
            timer = 0;
        }

    }

    /*private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }*/

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Laser")) {
            gameManager.IncrementScore();
            asteroidPool.Release(this);
            Destroy(other.gameObject);
            AudioManager.Instance.PlaySoundEffect(crashSound);
        }
        else if (other.gameObject.CompareTag("Player")) {
            asteroidPool.Release(this);
            AudioManager.Instance.PlaySoundEffect(crashSound);
        }
    }
}