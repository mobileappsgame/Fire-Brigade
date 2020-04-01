using System.Collections;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    // Номер уровня
    public static int LevelNumber;

    [Header("Анимация загрузки")]
    [SerializeField] private GameObject loading;

    // Ссылка на компонент
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
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
        yield return new WaitForSeconds(1.2f);
        transitions.GoToScene("Level " + number);
    }
}