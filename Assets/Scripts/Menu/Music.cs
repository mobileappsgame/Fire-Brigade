using System;
using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Ссылка на компонент
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Текущее значение сохраненного параметра
        Sound.soundActivity = PlayerPrefs.GetString("sounds") == "on" ? true : false;
        MusicSetting();
    }

    /// <summary>
    /// Включение/выключение фоновой музыки
    /// </summary>
    public void MusicSetting()
    {
        StopAllCoroutines();

        if (Sound.soundActivity)
            // Постепенно увеличиваем громкость фоновой музыки
            _ = StartCoroutine(ChangeVolume(0.1f, x => x < 0.9));
        else
            // Постепенно уменьшаем громкость фоновой музыки
            _ = StartCoroutine(ChangeVolume(-0.1f, x => x > 0));
    }

    /// <summary>
    /// Изменение громкости фоновой музыки
    /// </summary>
    /// <param name="value">значение для изменения</param>
    /// <param name="function">функция проверки громкости</param>
    public IEnumerator ChangeVolume(float value, Func<float, bool> function)
    {
        // Если громкость увелчивается, запускаем проигрывание
        if (value > 0) audioSource.Play();

        while (function(audioSource.volume))
        {
            yield return new WaitForSeconds(0.05f);
            audioSource.volume += value;
        }

        // Если громкость нулевая, останавливаем музыку
        if (audioSource.volume <= 0) audioSource.Stop();
    }
}