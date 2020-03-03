using System;
using System.Collections;
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
        if (activity)
        {
            // Останавливаем увеличение
            StopAllCoroutines();
            // Устанавливаем коэффициент
            coefficient = 0.2f;
        }
        else
        {
            // Запускаем увеличение коэффициента
            StartCoroutine(IncreaseCoefficient());
        }
    }

    /// <summary>
    /// Постепенное увеличение коэффициента
    /// </summary>
    private IEnumerator IncreaseCoefficient()
    {
        while (coefficient < 1)
        {
            yield return new WaitForSeconds(0.05f);
            coefficient += 0.1f;
        }
    }
}