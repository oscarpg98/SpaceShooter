using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class Laser : MonoBehaviour
{
    [SerializeField] private float velocidad;
    private ObjectPool<Laser> laserPool;
    public ObjectPool<Laser> LaserPool { get => laserPool; set { laserPool = value; } }
    private float timer;

    void Start() {
        // Roto el laser ya que en el sprite original está orientado verticalmente.
        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        velocidad = 15;
    }

    void Update() {
        transform.Translate(Time.deltaTime * velocidad * -transform.right);
        
        timer += Time.deltaTime;
        if (timer >= 4) {
            laserPool.Release(this);
            timer = 0;
        }
    }
}
