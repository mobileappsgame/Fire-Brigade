using System;
using UnityEngine;
using UnityEngine.UI;

public class StretcherStrength : MonoBehaviour
{
    // Изменение прочности носилок
    public static Action StrengthChange;

    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    // Ссылка на компонент текста
    private Text percent;

    private void Awake()
    {
        percent = GetComponent<Text>();

        // Добавляем вывод прочности носилок
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
        percent.text = stretcher.Strength > 0 ? stretcher.Strength.ToString() : "0";
        percent.text += "%";
    }
}