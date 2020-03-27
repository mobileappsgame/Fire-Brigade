using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Текущий игровой режим
    public static string Mode { get; set; } = "play";

    [Header("Максимум ошибок")]
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

        // Получаем количество жильцов
        victims = windowsManager.Victims;

        quantityErrors += ChangeErrors;
        quantityVictims += ChangeVictims;
    }

    /// <summary>
    /// Изменение количества ошибок
    /// </summary>
    /// <param name="value">значение</param>
    public void ChangeErrors(int value)
    {
        errors += value;

        // Если ошибок не осталось, завершаем уровень проигрышем
        if (errors <= 0) StartCoroutine(CompleteLevel("lose"));
    }

    /// <summary>
    /// Изменение количества жильцов
    /// </summary>
    /// <param name="value">значение</param>
    private void ChangeVictims(int value)
    {
        victims -= value;

        // Если жильцы закончились и ошибок немного, завершаем уровень победой
        if (victims <= 0 && errors > 0) StartCoroutine(CompleteLevel("victory"));
    }

    /// <summary>
    /// Завершение текущего уровня
    /// </summary>
    /// <param name="mode">режим завершения</param>
    private IEnumerator CompleteLevel(string mode)
    {
        if (Mode == "play")
        {
            yield return new WaitForSeconds(1.0f);

            Mode = mode;
            results.ShowResult();
        }
    }

    private void OnDestroy()
    {
        quantityErrors = null;
        quantityVictims = null;
    }
}