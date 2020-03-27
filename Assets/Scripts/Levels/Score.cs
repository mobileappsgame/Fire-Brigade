using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Текущий счет уровня
    public int ScoreLevel { get; private set; } = 0;

    // Изменение счета
    public static Action<int> ChangingScore;

    [Header("Текст изменения")]
    [SerializeField] private Text textChange;

    // Ссылки на компоненты
    private Text textScore;
    private Animator animator;
    private Outline outline;

    private void Awake()
    {
        textScore = GetComponent<Text>();
        animator = textChange.GetComponent<Animator>();
        outline = textChange.GetComponent<Outline>();

        ChangingScore += ChangeScore;
    }

    /// <summary>
    /// Изменение текущего счета
    /// </summary>
    /// <param name="value">значение</param>
    private void ChangeScore(int value)
    {
        ScoreLevel += value;

        // Запрещаем отрицательный счет
        if (ScoreLevel < 0) ScoreLevel = 0;

        UpdateLevelScore(value);
    }

    /// <summary>
    /// Отображение текущего счета
    /// </summary>
    /// <param name="value">значение</param>
    private void UpdateLevelScore(int value)
    {
        // Выводим счет уровня
        textScore.text = ScoreLevel.ToString();

        animator.enabled = true;
        // Записываем количество очков в эффект изменения
        textChange.text = (value > 0 ? "+ " : "") + value.ToString();
        // Устанавливаем обводку эффекта в зависимости от значения
        outline.effectColor = value > 0 ? Color.green : Color.red;
        // Перезапускаем анимацию
        animator.Rebind();
    }

    private void OnDestroy()
    {
        ChangingScore = null;
    }
}