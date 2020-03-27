using System;
using UnityEngine;
using UnityEngine.UI;

public class VictimsCounting : MonoBehaviour
{
    // Изменение количества персонажей
    public static Action QuantityChange;

    [Header("Менеджер окон")]
    [SerializeField] private WindowsManager windows;

    // Ссылка на компонент
    private Text quantity;

    private void Awake()
    {
        quantity = GetComponent<Text>();
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

    private void OnDestroy()
    {
        QuantityChange = null;
    }
}