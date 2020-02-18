using System;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    // Коэффициент замедления падения
    public static float coefficient = 1;

    // Делегат изменения коэффициента
    public static Action<bool> SlowDown;

    private void Start()
    {
        // Добавляем метод изменения коэффициента
        SlowDown += ChangeSlowdown;
    }

    /// <summary>
    /// Изменение коэффициента замедления
    /// </summary>
    /// <param name="activity">Активность замедления</param>
    private void ChangeSlowdown(bool activity)
    {
        // Устанавливаем необходимый коэффициент
        coefficient = activity ? 0.2f : 1;
    }
}