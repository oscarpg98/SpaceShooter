using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    private AudioSource musicAudioSource;
    private AudioSource effectsAudioSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float musicVolume = 0.2f;
    [SerializeField] private float effectsVolume = 0.8f;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        musicAudioSource = GetComponent<AudioSource>();
        effectsAudioSource = gameObject.AddComponent<AudioSource>();

        // Configura la música de fondo
        musicAudioSource.volume = musicVolume;
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();

        // Configura el volumen para los efectos de sonido
        effectsAudioSource.volume = effectsVolume;
    }

    public void PlaySoundEffect(AudioClip clip) {
        effectsAudioSource.PlayOneShot(clip);
    }
}
