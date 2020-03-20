using System;
using UnityEngine;
using UnityEngine.UI;

public class StretcherStrength : MonoBehaviour
{
    // Изменение прочности носилок
    public static Action StrengthChange;

    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    // Текущее значение прочности
    private int presentValue;

    // Ссылки на компоненты
    private Text percent;
    private Animator animator;

    private void Awake()
    {
        percent = GetComponent<Text>();
        animator = GetComponent<Animator>();

        presentValue = stretcher.Strength;
        StrengthChange += ShowPercent;
    }

    private void Start()
    {
        ShowPercent();
    }

    /// <summary>
    /// Отображение прочности носилок
    /// </summary>
    private void ShowPercent()
    {
        percent.text = (stretcher.Strength > 0 ? stretcher.Strength.ToString() : "0") + "%";
        SubtractionAnimation();
    }

    /// <summary>
    /// Анимация уменьшения прочности
    /// </summary>
    private void SubtractionAnimation()
    {
        if (stretcher.Strength < presentValue)
        {
            // Обновляем текущий процент
            presentValue = stretcher.Strength;

            // Анимация вычитания
            animator.enabled = true;
            animator.Rebind();
        }
    }

    private void OnDestroy()
    {
        StrengthChange = null;
    }
}