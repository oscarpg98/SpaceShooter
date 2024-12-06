using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingSystem : MonoBehaviour {
    [SerializeField] private Laser laserPrefab;
    [SerializeField]private float shootingRatio;
    private float timer;
    private ObjectPool<Laser> laserPool;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip laserSound;

    private void Awake() {
        laserPool = new ObjectPool<Laser>(CreateLaser, GetLaser, ReleaseLaser, DestroyLaser);
    }
   
    private Laser CreateLaser() {
        Laser laserCopy = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laserCopy.LaserPool = laserPool;
        return laserCopy;
    }

    private void GetLaser(Laser laser) {
        laser.transform.position = transform.position;
        laser.gameObject.SetActive(true);
    }

    private void ReleaseLaser(Laser laser) {
        laser.gameObject.SetActive(false);
    }

    private void DestroyLaser(Laser laser) {
        Destroy(laser.gameObject);
    }

    void Update() {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= shootingRatio) {
            laserPool.Get();
            timer = 0f;

            AudioManager.Instance.PlaySoundEffect(laserSound);
        }
    }
}
