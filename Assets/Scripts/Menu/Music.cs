using UnityEngine;

public class Music : MonoBehaviour
{
    // Ссылка на компонент
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        MusicSetting();
    }

    /// <summary>
    /// Включение/выключение фоновой музыки
    /// </summary>
    public void MusicSetting()
    {
        if (Sound.soundActivity)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}