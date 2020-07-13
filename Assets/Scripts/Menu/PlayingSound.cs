using Cubra.Controllers;
using UnityEngine;

namespace Cubra
{
    public class PlayingSound : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Воспроизведение звука
        /// </summary>
        public void PlaySound()
        {
            if (SoundController.Activity) _audioSource.Play();
        }
    }
}