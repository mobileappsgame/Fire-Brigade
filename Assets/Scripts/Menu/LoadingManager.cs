using UnityEngine;
using GooglePlayGames;

namespace Cubra
{
    public class LoadingManager : MonoBehaviour
    {
        private void Awake()
        {
            #region Saved Data
            // Если отсутствуют сохранения
            if (!PlayerPrefs.HasKey("saved-data"))
            {
                // Перевод игры
                PlayerPrefs.SetString("language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru-RU" : "en-US");
                // Настройка звука
                PlayerPrefs.SetString("sounds", "on");

                // Игровой прогресс
                PlayerPrefs.SetInt("progress", 1);
                // Общий игровой счет
                PlayerPrefs.SetInt("total-score", 0);
                // Текущий счет для улучшений
                PlayerPrefs.SetInt("current-score", 0);

                // Уровень носилок
                PlayerPrefs.SetInt("stretcher", 1);
                // Количество улучшенных носилок
                PlayerPrefs.SetInt("super-stretcher", 2);
                // Использование улучшенных носилок
                PlayerPrefs.SetString("use-bonus", "no");
                // Успешное тушение носилок
                PlayerPrefs.SetString("fire-stretcher", "no");

                // Количество спасенных жильцов
                PlayerPrefs.SetInt("victims", 0);

                // Прохождение обучения
                PlayerPrefs.SetString("training", "no");

                // Первоначальное сохранение данных
                PlayerPrefs.SetString("saved-data", "yes");
            }
            #endregion

            // Активируем сервисы Google Play
            _ = PlayGamesPlatform.Activate();
        }

        private void Start()
        {
            var transitions = gameObject.GetComponent<TransitionsManager>();
            _ = StartCoroutine(transitions.GoToSceneWithPause(2f, (int)TransitionsManager.Scenes.Menu));
        }
    }
}