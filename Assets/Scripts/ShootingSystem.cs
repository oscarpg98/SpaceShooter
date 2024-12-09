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
    [SerializeField] private Transform[] spawnPositions;
    private bool tripleShotActive = false;

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
    public void ReleaseLaserExternally(Laser laser) {
        if (laser != null && laser.gameObject.activeSelf) {
            laserPool.Release(laser);
        }
        else {
            Debug.LogWarning("Intentando liberar un láser ya liberado.");
        }
    }


    void Update() {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= shootingRatio) {
            if (tripleShotActive) {
                foreach (Transform spawn in spawnPositions) {
                    FireLaser(spawn.position);
                }
            }
            else {
                laserPool.Get();
            }
            timer = 0f;
            AudioManager.Instance.PlaySoundEffect(laserSound);
        }
    }

    private void FireLaser(Vector3 spawnPosition) {
        Laser laser = laserPool.Get();
        laser.transform.position = spawnPosition;
        laser.gameObject.SetActive(true);
    }

    public void ActivateTripleShot(float duration) {
        StartCoroutine(TripleShot(duration));
    }

    private IEnumerator TripleShot(float duration) {
        tripleShotActive = true;
        yield return new WaitForSeconds(duration);
        tripleShotActive = false;
    }
}
