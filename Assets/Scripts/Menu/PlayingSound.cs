using UnityEngine;

public class PlayingSound : MonoBehaviour
{
    // Ссылка на компонент
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Воспроизведение звука
    /// </summary>
    public void PlaySound()
    {
        if (Sound.soundActivity) audioSource.Play();
    }
}