using UnityEngine;

public class StretcherChange : MonoBehaviour
{
    [Header("Панель выбора")]
    [SerializeField] private Animator panel;

    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонажи касаются пожарной машины
        if (collision.gameObject.GetComponent<Control>())
        {
            // Активируем компонент анимации
            if (panel.enabled == false) panel.enabled = true;

            // Открываем панель выбора
            panel.SetBool("Opening", true);

            // Уменьшаем коэффициент замедления
            Slowdown.SlowDown?.Invoke(true);

            // Ремонтируем (увеличиваем) прочность носилок
            stretcher.Coroutine = StartCoroutine(stretcher.IncreaseStrength());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Control>())
        {
            // Закрываем панель выбора
            panel.SetBool("Opening", false);

            // Увеличиваем коэффициент замедления
            Slowdown.SlowDown?.Invoke(false);

            // Останавливаем увеличение прочности носилок
            if (stretcher.Coroutine != null)
                StopCoroutine(stretcher.Coroutine);
        }
    }
}