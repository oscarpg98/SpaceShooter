using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    private AudioSource musicAudioSource; // Para la m�sica
    private AudioSource effectsAudioSource; // Para los efectos de sonido

    [SerializeField] private AudioClip musicClip; // Clip de m�sica
    [SerializeField] private float musicVolume = 0.2f; // Volumen de la m�sica
    [SerializeField] private float effectsVolume = 0.8f; // Volumen de los efectos de sonido

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        musicAudioSource = GetComponent<AudioSource>(); // Asigna el primer AudioSource
        effectsAudioSource = gameObject.AddComponent<AudioSource>(); // Crea el segundo AudioSource para los efectos

        // Configura el volumen de la m�sica desde el inspector
        musicAudioSource.volume = musicVolume;
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true; // Para que la m�sica se repita
        musicAudioSource.Play(); // Inicia la m�sica

        // Configura el volumen para los efectos de sonido
        effectsAudioSource.volume = effectsVolume;
    }

    // Funci�n para reproducir efectos de sonido (como el l�ser)
    public void PlaySoundEffect(AudioClip clip) {
        effectsAudioSource.PlayOneShot(clip); // Reproduce el sonido con el volumen de efectos
    }
}
