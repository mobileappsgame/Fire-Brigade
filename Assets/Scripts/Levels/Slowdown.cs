using System;
using System.Collections;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    // Коэффициент замедления падения
    public static float coefficient = 1;

    // Изменение коэффициента падения
    public static Action<bool> SlowDown;

    private void Start()
    {
        SlowDown += ChangeSlowdown;
    }

    /// <summary>
    /// Изменение коэффициента замедления
    /// </summary>
    /// <param name="activity">активность замедления</param>
    private void ChangeSlowdown(bool activity)
    {
        if (activity)
        {
            StopAllCoroutines();

            // Замедляем падение
            coefficient = 0.25f;
        }
        else
        {
            // Запускаем восстановление коэффициента
            StartCoroutine(IncreaseCoefficient());
        }
    }

    /// <summary>
    /// Постепенное восстановление коэффициента падения
    /// </summary>
    private IEnumerator IncreaseCoefficient()
    {
        while (coefficient < 1)
        {
            yield return new WaitForSeconds(0.05f);
            coefficient += 0.1f;
        }
    }

    private void OnDestroy()
    {
        SlowDown = null;
    }
}