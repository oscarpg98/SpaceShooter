using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AsteroidSpawner : MonoBehaviour {
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnTime = 0.5f;
    private float timer;
    private Camera mainCamera;
    private ObjectPool<Asteroid> asteroidPool;
    public ObjectPool<Asteroid> AsteroidPool { get => asteroidPool; set { asteroidPool = value; } }
    private float offset;

    private void Awake() {
        asteroidPool = new ObjectPool<Asteroid>(CreateAsteroid, GetAsteroid, ReleaseAsteroid, DestroyAsteroid);
    }
    void Start() {
        mainCamera = Camera.main;
        timer = 0f;
        offset = 2.1f;
    }
    private Asteroid CreateAsteroid() {
        Asteroid asteroidCopy = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        asteroidCopy.AsteroidPool = asteroidPool;
        return asteroidCopy;

    }
    private void GetAsteroid(Asteroid asteroid) {
        asteroid.gameObject.SetActive(true);
        float spawnX = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect + 1;
        float spawnY = UnityEngine.Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize - offset);
        asteroid.transform.position = new Vector3(spawnX, spawnY, 0);
    }
    private void ReleaseAsteroid(Asteroid asteroid) {
        asteroid.gameObject.SetActive(false);
    }

    private void DestroyAsteroid(Asteroid asteroid) {
        Destroy(asteroid.gameObject);
    }


    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnTime) {
            asteroidPool.Get();
            timer = 0f;
        }
    }
}
