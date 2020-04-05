using System.Collections;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    // Номер уровня
    public static int LevelNumber;

    [Header("Анимация загрузки")]
    [SerializeField] private GameObject loading;

    // Возврат с уровня (для включения музыки)
    public static bool ReturnLevel;

    // Ссылки на компоненты
    private Transitions transitions;
    private Music backgroundMusic;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
        backgroundMusic = FindObjectOfType<Music>();
    }

    private void Start()
    {
        if (Sound.soundActivity && ReturnLevel)
        {
            ReturnLevel = false;
            // Постепенно увеличиваем громкость фоновой музыки
            _ = StartCoroutine(backgroundMusic.ChangeVolume(0.1f, x => x < 0.9));
        }
    }

    /// <summary>
    /// Открытие выбранного уровня
    /// </summary>
    /// <param name="level">кнопка уровня</param>
    public void OpenLevel(Level level)
    {
        // Активируем анимацию
        loading.SetActive(true);
        loading.GetComponent<Animator>().Rebind();

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
        _ = StartCoroutine(backgroundMusic.ChangeVolume(-0.1f, x => x > 0));
        // Указываем, что был переход на уровень
        ReturnLevel = true;

        yield return new WaitForSeconds(1.0f);
        transitions.GoToScene("Level " + number);
    }
}