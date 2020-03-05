using System;
using UnityEngine;
using UnityEngine.UI;

public class VictimsCounting : MonoBehaviour
{
    // Изменение количества персонажей
    public static Action QuantityChange;

    [Header("Компонент окон")]
    [SerializeField] private WindowsManager windows;

    // Ссылка на компонент текста
    private Text quantity;

    private void Awake()
    {
        quantity = GetComponent<Text>();

        // Добавляем вывод количества персонажей
        QuantityChange += ShowQuantity;
    }

    private void Start()
    {
        ShowQuantity();
    }

    /// <summary>
    /// Отображение количества оставшихся персонажей
    /// </summary>
    private void ShowQuantity()
    {
        quantity.text = windows.Victims.ToString();
    }
}