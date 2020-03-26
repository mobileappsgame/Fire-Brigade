using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Текущий игровой режим
    public static string Mode { get; set; } = "play";

    [Header("Доступных ошибок")]
    [SerializeField] private int errors;

    // Изменение количества ошибок
    public static Action<int> quantityErrors;

    // Количество жильцов
    private int victims;

    // Проверка количеста жильцов
    public static Action<int> quantityVictims;

    [Header("Менеджер окон")]
    [SerializeField] private WindowsManager windowsManager;

    // Ссылка на компонент
    private Results results;

    private void Awake()
    {
        results = Camera.main.GetComponent<Results>();

        // Получаем колиество жильцов
        victims = windowsManager.Victims;

        quantityErrors += ChangeErrors;
        quantityVictims += CheckQuantityVictims;
    }

    /// <summary>
    /// Изменение количества ошибок
    /// </summary>
    /// <param name="value">Значение</param>
    public void ChangeErrors(int value)
    {
        errors += value;

        // Если ошибок не осталось, завершаем уровень
        if (errors <= 0) StartCoroutine(CompleteLevel("lose"));
    }

    /// <summary>
    /// Проверка количества жильцов и ошибок
    /// </summary>
    /// <param name="value">Значение</param>
    private void CheckQuantityVictims(int value)
    {
        victims -= value;

        // Если жильцы закончились, завершаем уровень
        if (victims <= 0 && errors > 0) StartCoroutine(CompleteLevel("victory"));
    }

    /// <summary>
    /// Завершение текущего уровня
    /// </summary>
    /// <param name="mode">Режим завершения</param>
    private IEnumerator CompleteLevel(string mode)
    {
        if (Mode == "play")
        {
            yield return new WaitForSeconds(1.0f);

            Mode = mode;
            // Выводим результаты
            results.ShowResult();
        }
    }

    private void OnDestroy()
    {
        quantityErrors = null;
        quantityVictims = null;
    }
}