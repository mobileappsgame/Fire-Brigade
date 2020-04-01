using System.Collections;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    // Коэффициент замедления падения
    public static float coefficient;

    private void Start()
    {
        coefficient = 1;
    }

    /// <summary>
    /// Изменение коэффициента замедления
    /// </summary>
    /// <param name="activity">активность замедления</param>
    public void ChangeSlowdown(bool activity)
    {
        if (activity)
        {
            StopAllCoroutines();

            // Замедляем падение
            coefficient = 0.35f;
        }
        else
        {
            // Запускаем восстановление коэффициента
            _ = StartCoroutine(IncreaseCoefficient());
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
}