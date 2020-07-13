using System.Collections;
using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class LevelSelection : MonoBehaviour
    {
        // Номер уровня
        public static int LevelNumber;

        // Возврат с уровня (для включения музыки)
        public static bool ReturnLevel;

        [Header("Анимация загрузки")]
        [SerializeField] private GameObject _loading;

        private TransitionsManager _transitionsManager;
        private BackgroundMusic _backgroundMusic;

        private void Awake()
        {
            _transitionsManager = Camera.main.GetComponent<TransitionsManager>();
            _backgroundMusic = FindObjectOfType<BackgroundMusic>();
        }

        private void Start()
        {
            if (SoundController.Activity && ReturnLevel)
            {
                ReturnLevel = false;
                // Постепенно увеличиваем громкость фоновой музыки
                _ = StartCoroutine(_backgroundMusic.ChangeVolume(0.1f, x => x < 0.9));
            }
        }

        /// <summary>
        /// Открытие выбранного уровня
        /// </summary>
        /// <param name="level">кнопка уровня</param>
        public void OpenLevel(Level level)
        {
            // Активируем анимацию
            _loading.SetActive(true);
            _loading.GetComponent<Animator>().Rebind();

            // Записываем номер уровня
            LevelNumber = level.Number;
            // Запускаем переход на сцену
            _ = StartCoroutine(LaunchLoading(level.Number.ToString()));
        }

        /// <summary>
        /// Запуск загрузки уровня
        /// </summary>
        private IEnumerator LaunchLoading(string number)
        {
            // Постепенно уменьшаем громкость фоновой музыки
            _ = StartCoroutine(_backgroundMusic.ChangeVolume(-0.1f, x => x > 0));
            // Указываем, что был переход на уровень
            ReturnLevel = true;

            yield return new WaitForSeconds(1.0f);
            _transitionsManager.GoToScene("Level " + number);
        }
    }
}