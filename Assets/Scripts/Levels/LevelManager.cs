using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Текущий игровой режим
    public static string GameMode { get; private set; }

    [Header("Всего жителей")]
    [SerializeField] private int victims;

    public int Victims { get => victims; set { victims = value; } }

    // Текущее количество жителей
    public int CurrentVictims { get; private set; }

    public delegate void ValueChanged();
    // Событие по изменению количества жителей
    public event ValueChanged VictimsChanged;

    [Header("Максимум ошибок")]
    [SerializeField] private int errors;

    // Текущее количество ошибок
    private int currentErrors = 0;

    // Текущий счет уровня
    public int Score { get; private set; }

    public delegate void ScoreChanged(int value);
    // Событие по изменению игрового счета
    public event ScoreChanged ScoresChanged;

    // Ссылка на компонент
    private Results results;

    private void Awake()
    {
        GameMode = "play";
        CurrentVictims = victims;

        results = Camera.main.GetComponent<Results>();
    }

    /// <summary>
    /// Уменьшение количества жильцов
    /// </summary>
    public void ReduceQuantityVictims()
    {
        CurrentVictims--;

        // Сообщаем об изменении
        VictimsChanged?.Invoke();

        // Если жильцы закончились и ошибок меньше допустимого
        if (CurrentVictims <= 0 && currentErrors < errors)
            // Завершаем текущий уровень победой
            _ = StartCoroutine(CompleteLevel("victory"));
    }

    /// <summary>
    /// Изменение количества ошибок
    /// </summary>
    /// <param name="value">значение</param>
    public void ChangeErrors(int value)
    {
        currentErrors += value;

        // Если набрано максимум ошибок
        if (currentErrors >= errors)
            // Завершаем уровень проигрышем
            _ = StartCoroutine(CompleteLevel("lose"));
    }

    /// <summary>
    /// Изменение счета уровня
    /// </summary>
    /// <param name="value">значение для изменения</param>
    public void ChangeScore(int value)
    {
        Score += value;

        // Сбрасываем отрицательное значение
        if (Score < 0) Score = 0;

        // Сообщаем об изменении
        ScoresChanged?.Invoke(value);
    }

    /// <summary>
    /// Завершение текущего уровня
    /// </summary>
    /// <param name="mode">режим завершения</param>
    private IEnumerator CompleteLevel(string mode)
    {
        if (GameMode == "play")
        {
            yield return new WaitForSeconds(1.2f);

            GameMode = mode;
            results.ShowResult();
        }
    }

    private void OnDestroy()
    {
        // Сброс подписчиков
        VictimsChanged = null;
        ScoresChanged = null;
    }
}