using System.Collections;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
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
    /// <param name="level">Кнопка уровня</param>
    public void OpenLevel(Level level)
    {
        // Активируем анимацию загрузки
        loading.SetActive(true);
        loading.GetComponent<Animator>().Rebind();

        // Запускаем переход на сцену
        StartCoroutine(LaunchLoading(level.Number));
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