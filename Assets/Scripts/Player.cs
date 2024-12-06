using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {
    [SerializeField] private int movementSpeed;
    [SerializeField] private int vida;
    private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private GameManager gameManager;


    void Start() {
        mainCamera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        Movimiento();
    }

    void Movimiento() {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(inputH, inputV, 0) * movementSpeed * Time.deltaTime, Space.World);

        ClampPosition();
    }

    void ClampPosition() {
        // Obtiene los bordes de la cámara.
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        float halfWidth = boxCollider.bounds.extents.x;
        float halfHeight = boxCollider.bounds.extents.y;

        Vector3 clampedPosition = transform.position;

        // Hace que el jugador no se salga de los bordes.
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = clampedPosition;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            gameManager.DecreaseHealth();
        }
    }
}