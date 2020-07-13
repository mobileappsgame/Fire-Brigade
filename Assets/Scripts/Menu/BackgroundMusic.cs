using System;
using System.Collections;
using Cubra.Controllers;
using UnityEngine;

namespace Cubra
{
    public class BackgroundMusic : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SwitchBackgroundMusic();
        }

        /// <summary>
        /// Включение/выключение фоновой музыки
        /// </summary>
        public void SwitchBackgroundMusic()
        {
            if (SoundController.Activity)
            {
                // Постепенное увеличение громкости музыки
                _ = StartCoroutine(ChangeVolume(0.1f, x => x < 0.9));
            }
            else
            {
                // Постепенное уменьшение громкости музыки
                _ = StartCoroutine(ChangeVolume(-0.1f, x => x > 0));
            }  
        }

        /// <summary>
        /// Изменение громкости фоновой музыки
        /// </summary>
        /// <param name="value">значение для изменения</param>
        /// <param name="function">функция проверки громкости</param>
        public IEnumerator ChangeVolume(float value, Func<float, bool> function)
        {
            if (value > 0) _audioSource.Play();

            while (function(_audioSource.volume))
            {
                yield return new WaitForSeconds(0.05f);
                _audioSource.volume += value;
            }

            if (_audioSource.volume <= 0) _audioSource.Stop();
        }
    }
}