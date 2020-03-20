using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Счет на уровне
    private int score = 0;

    // Изменение счета
    public static Action<int> ChangingScore;

    [Header("Эффект изменения")]
    [SerializeField] private Text textChange;

    // Ссылки на компоненты
    private Text textScore;
    private Animator animatorChange;
    private Outline outlineChange;

    private void Awake()
    {
        textScore = GetComponent<Text>();
        animatorChange = textChange.GetComponent<Animator>();
        outlineChange = textChange.GetComponent<Outline>();

        ChangingScore += ChangeScore;
    }

    /// <summary>
    /// Изменение счета
    /// </summary>
    /// <param name="value">Значение</param>
    private void ChangeScore(int value)
    {
        score += value;
        if (score < 0) score = 0;

        UpdateLevelScore(value);
    }

    /// <summary>
    /// Отображение счета на уровне
    /// </summary>
    /// <param name="value"></param>
    private void UpdateLevelScore(int value)
    {
        // Выводим счет уровня
        textScore.text = score.ToString();

        // Активируем анимацию
        animatorChange.enabled = true;
        // Записываем количество очков в эффект изменения
        textChange.text = (value > 0 ? "+ " : "") + value.ToString();
        // Устанавливаем обводку эффекта
        outlineChange.effectColor = value > 0 ? Color.green : Color.red;
        // Перезапускаем анимацию
        animatorChange.Rebind();
    }

    private void OnDestroy()
    {
        ChangingScore = null;
    }
}