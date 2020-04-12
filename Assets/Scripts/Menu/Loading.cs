using System.Collections;
using UnityEngine;
using GooglePlayGames;

public class Loading : MonoBehaviour
{
    // Ссылка на компонент
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
    }

    private void Start()
    {
        #region SaveData
        // Перевод интерфейса игры
        if (!PlayerPrefs.HasKey("language"))
            PlayerPrefs.SetString("language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru-RU" : "en-US");
        // Звуки и музыка
        if (!PlayerPrefs.HasKey("sounds")) PlayerPrefs.SetString("sounds", "on");

        // Игровой прогресс
        if (!PlayerPrefs.HasKey("progress")) PlayerPrefs.SetInt("progress", 1);

        // Общий счет
        if (!PlayerPrefs.HasKey("total-score")) PlayerPrefs.SetInt("total-score", 0);
        // Текущее количество очков для улучшений
        if (!PlayerPrefs.HasKey("current-score")) PlayerPrefs.SetInt("current-score", 0);

        // Уровень носилок
        if (!PlayerPrefs.HasKey("stretcher")) PlayerPrefs.SetInt("stretcher", 1);
        // Количество улучшенных носилок
        if (!PlayerPrefs.HasKey("super-stretcher")) PlayerPrefs.SetInt("super-stretcher", 2);
        // Использование улучшенных носилок
        if (!PlayerPrefs.HasKey("use-bonus")) PlayerPrefs.SetString("use-bonus", "no");
        // Успешное тушение носилок
        if (!PlayerPrefs.HasKey("fire-stretcher")) PlayerPrefs.SetString("fire-stretcher", "no");

        // Количество пойманных жильцов
        if (!PlayerPrefs.HasKey("victims")) PlayerPrefs.SetInt("victims", 0);

        // Прохождение обучения
        if (!PlayerPrefs.HasKey("training")) PlayerPrefs.SetString("training", "no");
        #endregion

        // Активируем сервисы Google Play
        _ = PlayGamesPlatform.Activate();

        _ = StartCoroutine(GoToMenu());
    }

    /// <summary>
    /// Переход в главное меню
    /// </summary>
    private IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2.0f);
        transitions.GoToScene(Transitions.Scenes.Menu);
    }
}